using SupportHelper.Communication.Dtos.CouchDbDto;
using SupportHelper.Domain.Aggregates;
using SupportHelper.Domain.Entities;
using SupportHelper.Domain.Interfaces.Repositories.Database;
using SupportHelper.Domain.Interfaces.Repositories.Memory;
using SupportHelper.Domain.ValueObjects;
using SupportHelper.Exceptions.ExceptionsBase;
using SupportHelper.Infrastructure.Data.CouchDB.Json;
using SupportHelper.Infrastructure.Data.CouchDB.Json.Responses;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace SupportHelper.Infrastructure.Data.CouchDB.Repositories.Database
{
    public sealed class MachineRepository : IMachineRepository
    {
        private readonly ITokenMemoryRepository _tokenMemoryRepository;
        private readonly HttpClient _client;

        public MachineRepository(IHttpClientFactory clientFactory, ITokenMemoryRepository tokenMemoryRepository)
        {
            _tokenMemoryRepository = tokenMemoryRepository;
            _client = clientFactory.CreateClient("CouchDB");
        }

        public async Task Login()
        {
            try
            {
                var value = new
                {
                    name = "admin",
                    password = "admin"
                };
                var content = new StringContent(JsonSerializer.Serialize(value), Encoding.UTF8, "application/json");
                var response = await _client.PostAsync("_session", content);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<AllDocsDto> GetAllAsync(int limit, int skip)
        {
            try
            {
                HttpResponseMessage response = await _client.GetAsync("machine-dev-db/machine_all_docs");
                if (!response.IsSuccessStatusCode)
                {
                    throw new GenericErrorException(["HOUVE UMA RESPOSTA HTTP NEGATIVA"]);
                }
                Stream stream = await response.Content.ReadAsStreamAsync();
                return await JsonSerializer.DeserializeAsync<AllDocsDto>(stream)
                    ?? throw new GenericErrorException(["Houve um erro"]);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<Machine> GetByHostname(string hostname)
        {
            var findData = await FindByHostnameAsync(hostname);
            if (findData == null || findData.Docs == null)
            {
                throw new Exception();
            }
            var doc = findData.Docs.FirstOrDefault() ?? throw new Exception();
            return doc.Machine;
        }

        public async Task<string> GetConnectionByHostnameAsync(string hostname)
        {
            var doc = await FindDocByHostnameAsync(hostname);
            var connId = doc.SignalR.ConnectionId;
            return connId;
        }

        public async Task InsertAsync(MachineSchemaJson schema)
        {
            try
            {
                HttpRequestMessage httpRequest = new(HttpMethod.Put, $"{schema.Machine.Id}");
                httpRequest.Headers.Authorization = new AuthenticationHeaderValue("AuthSession", "");
                string json = JsonSerializer.Serialize(schema);
                httpRequest.Content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await _client.SendAsync(httpRequest);
                if (!response.IsSuccessStatusCode)
                {
                    throw new GenericErrorException(["OCORREU UM ERRO GENERICO"]);
                }
                Stream stream = await response.Content.ReadAsStreamAsync();
                ResponseBaseDto content = await JsonSerializer.DeserializeAsync<ResponseBaseDto>(stream)
                    ?? throw new GenericErrorException(["ERROR!"]);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<ResponseBaseDto> InsertOrUpdateAsync(MachineSchemaJson schema)
        {
            var doc = await FindDocByHostnameAsync(schema.Machine.Hostname.ToLower());
            try
            {
                doc.Update(schema);
                string json = JsonSerializer.Serialize(doc);
                HttpRequestMessage httpRequest = new(HttpMethod.Put, $"machine-dev-db/{doc.Id}")
                {
                    Content = new StringContent(json, Encoding.UTF8, "application/json")
                };
                HttpResponseMessage response = await _client.SendAsync(httpRequest);
                if (!response.IsSuccessStatusCode)
                {
                    throw new GenericErrorException(["OCORREU UM ERRO GENERICO"]);
                }
                Stream stream = await response.Content.ReadAsStreamAsync();
                return await JsonSerializer.DeserializeAsync<ResponseBaseDto>(stream)
                    ?? throw new GenericErrorException(["ERROR!"]);
            }
            catch (Exception ex)
            {
                throw new NotImplementedException();
            }
        }

        public async Task InsertOrUpdateAsync(string hostname, string connId)
        {
            try
            {
                var findReponse = await FindByHostnameAsync(hostname);
                if (findReponse != null && findReponse.Docs.Any())
                {
                    var doc = findReponse.Docs.FirstOrDefault();
                    doc.SignalR = new SignalR(connId, "");
                    var content = new StringContent(JsonSerializer.Serialize(doc), Encoding.UTF8, "application/json");
                    await SendToDatabase(content, doc.Id);
                    return;
                }
                var machine = Machine.Create(hostname);
                var signalR = new SignalR(connId, "");
                var machineSchemaJson = MachineSchemaJson.Create(machine, signalR);
                var json = new StringContent(JsonSerializer.Serialize(machineSchemaJson), Encoding.UTF8, "application/json");
                await SendToDatabase(json, machine.Hostname);
                return;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task UpdateAsync(MachineSchemaJson schema)
        {
            try
            {
                HttpRequestMessage request = new(HttpMethod.Put, "");
                request.Headers.Authorization = new AuthenticationHeaderValue("AuthSession", "");
                string json = JsonSerializer.Serialize(schema);
                request.Content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _client.SendAsync(request);
                if (!response.IsSuccessStatusCode)
                {
                    var stream = await response.Content.ReadAsStreamAsync();
                    var errorBaseDto = await JsonSerializer.DeserializeAsync<ErrorBaseDto>(stream)
                        ?? throw new GenericErrorException(["NÃO FOI POSSIVEL DESERIALIZAR OBJETO"]);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        private async Task<FindDataResponse?> FindByHostnameAsync(string hostname)
        {
            try
            {
                var query = new
                {
                    selector = new
                    {
                        Machine = new
                        {
                            Hostname = hostname
                        }
                    }
                };

                var content = new StringContent(JsonSerializer.Serialize(query), Encoding.UTF8, "application/json");
                var response = await _client.PostAsync("machine-dev-db/_find", content);
                if (!response.IsSuccessStatusCode)
                {
                    throw new NotImplementedException();
                }
                return await JsonSerializer.DeserializeAsync<FindDataResponse>(
                    await response.Content.ReadAsStreamAsync());
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private async Task<Doc> FindDocByHostnameAsync(string hostname)
        {
            var query = new
            {
                selector = new
                {
                    Machine = new
                    {
                        Hostname = hostname
                    }
                }
            };

            var content = new StringContent(JsonSerializer.Serialize(query), Encoding.UTF8, "application/json");
            var response = await _client.PostAsync("machine-dev-db/_find", content);
            if (!response.IsSuccessStatusCode)
            {
                throw new NotImplementedException();
            }
            var findResponse = await JsonSerializer.DeserializeAsync<FindDataResponse>(
                await response.Content.ReadAsStreamAsync()) ?? throw new NotImplementedException();

            return findResponse.Docs.FirstOrDefault() ?? throw new Exception("Não encontrado nenhum documento");

        }

        private async Task SendToDatabase(StringContent content, string? docId = null)
        {
            var response = await _client.PutAsync($"machine-dev-db/{docId}", content);
            if (!response.IsSuccessStatusCode)
                throw new NotImplementedException();
            return;
        }
    }
}
