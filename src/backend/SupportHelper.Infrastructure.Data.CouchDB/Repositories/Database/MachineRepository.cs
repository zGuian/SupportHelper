using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
using System.Net.Mail;
using System.Text;

namespace SupportHelper.Infrastructure.Data.CouchDB.Repositories.Database
{
    public sealed class MachineRepository : IMachineRepository
    {
        private readonly ITokenMemoryRepository _tokenMemoryRepository;
        private readonly ILogger<MachineRepository> _logger;
        private readonly HttpClient _client;

        public MachineRepository(IHttpClientFactory clientFactory, ITokenMemoryRepository tokenMemoryRepository, ILogger<MachineRepository> logger)
        {
            _tokenMemoryRepository = tokenMemoryRepository;
            _client = clientFactory.CreateClient("CouchDB");
            _logger = logger;
        }

        public async Task<(AllDocsDto, int)> GetAllAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default)
        {
            try
            {
                var responseTask = _client.GetAsync("machine-dev-db/_all_docs", cancellationToken);
                var countTask = _client.GetAsync("machine-dev-db/", cancellationToken);
                var result = await Task.WhenAll(responseTask, countTask);
                if (!result[0].IsSuccessStatusCode && !result[1].IsSuccessStatusCode)
                {
                    throw new GenericErrorException(["HOUVE UMA RESPOSTA HTTP NEGATIVA"]);
                }
                var jsonString = await result[1].Content.ReadAsStringAsync(cancellationToken);
                var json = JObject.Parse(jsonString);
                int count = json["doc_count"].Value<int>();

                jsonString = await result[0].Content.ReadAsStringAsync(cancellationToken);
                var alldocs = JsonConvert.DeserializeObject<AllDocsDto>(jsonString)
                    ?? throw new JsonException("Ocorreu um problema ao deserializar objeto");
                return (alldocs, count);
            }
            catch (Exception ex)
            {
                _logger.LogError("ERROR: {ex}", ex.Message);
                throw;
            }
        }

        public async Task<MachineSchemaJson> GetByHostnameAsync(string hostname, CancellationToken cancellationToken = default)
        {
            var findData = await FindDocByHostnameAsync(hostname, cancellationToken) ?? throw new Exception();
            var doc = findData;
            return MachineSchemaJson.Create(doc.Machine, doc.SignalR);
        }

        public async Task<string> GetConnectionByHostnameAsync(string hostname, CancellationToken cancellationToken = default)
        {
            var doc = await FindDocByHostnameAsync(hostname, cancellationToken);
            var connId = doc.SignalR.ConnectionId;
            return connId;
        }

        public async Task InsertAsync(MachineSchemaJson schema, CancellationToken cancellationToken = default)
        {
            try
            {
                HttpRequestMessage httpRequest = new(HttpMethod.Put, $"{schema.Machine.Id}");
                httpRequest.Headers.Authorization = new AuthenticationHeaderValue("AuthSession", "");
                string json = JsonConvert.SerializeObject(schema);
                httpRequest.Content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await _client.SendAsync(httpRequest, cancellationToken);
                if (!response.IsSuccessStatusCode)
                {
                    throw new GenericErrorException(["OCORREU UM ERRO GENERICO"]);
                }
                var jsonString = await response.Content.ReadAsStringAsync(cancellationToken);
                ResponseBaseDto content =
                    JsonConvert.DeserializeObject<ResponseBaseDto>(jsonString)
                        ?? throw new GenericErrorException(["ERROR!"]);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<ResponseBaseDto> InsertOrUpdateAsync(MachineSchemaJson schema, CancellationToken cancellationToken = default)
        {
            try
            {
                var doc = await FindDocByHostnameAsync(schema.Machine.Hostname, cancellationToken);
                doc.Update(schema);
                string json = JsonConvert.SerializeObject(doc);
                HttpRequestMessage httpRequest = new(HttpMethod.Put, $"machine-dev-db/{doc.Id}")
                {
                    Content = new StringContent(json, Encoding.UTF8, "application/json")
                };

                HttpResponseMessage response = await _client.SendAsync(httpRequest, cancellationToken);
                if (!response.IsSuccessStatusCode)
                {
                    throw new GenericErrorException(["OCORREU UM ERRO GENERICO"]);
                }
                var stream = await response.Content.ReadAsStringAsync(cancellationToken);
                return JsonConvert.DeserializeObject<ResponseBaseDto>(stream)
                    ?? throw new GenericErrorException(["ERROR!"]);
            }
            catch (Exception ex)
            {
                throw new NotImplementedException();
            }
        }

        public async Task InsertOrUpdateAsync(string hostname, string connId, CancellationToken cancellationToken = default)
        {
            try
            {
                var findReponse = await FindByHostnameAsync(hostname, cancellationToken);
                if (findReponse != null && findReponse.Docs.Any())
                {
                    var doc = findReponse.Docs.FirstOrDefault() ?? throw new NotImplementedException();
                    doc.SignalR = new SignalR(connId, DateTime.Now.ToString("dd/MM/yy-HH:mm:ss"));
                    var content = new StringContent(JsonConvert.SerializeObject(doc), Encoding.UTF8, "application/json");
                    await SendToDatabase(content, doc.Id, cancellationToken);
                    return;
                }
                var machine = Machine.Create(hostname);
                var signalR = new SignalR(connId, string.Empty);
                var machineSchemaJson = MachineSchemaJson.Create(machine, signalR);
                var json = new StringContent(JsonConvert.SerializeObject(machineSchemaJson), Encoding.UTF8, "application/json");
                await SendToDatabase(json, machine.Hostname, cancellationToken);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task UpdateAsync(MachineSchemaJson schema, CancellationToken cancellationToken = default)
        {
            try
            {
                HttpRequestMessage request = new(HttpMethod.Put, "");
                request.Headers.Authorization = new AuthenticationHeaderValue("AuthSession", "");
                var json = JsonConvert.SerializeObject(schema);
                request.Content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _client.SendAsync(request, cancellationToken);
                if (!response.IsSuccessStatusCode)
                {
                    var jsonString = await response.Content.ReadAsStringAsync(cancellationToken);
                    var errorBaseDto = JsonConvert.DeserializeObject<ErrorBaseDto>(jsonString);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private async Task<FindDataResponse?> FindByHostnameAsync(string hostname, CancellationToken cancellationToken = default)
        {
            try
            {
                var query = new
                {
                    selector = new
                    {
                        Machine = new
                        {
                            Hostname = hostname.ToLower()
                        }
                    }
                };

                var content = new StringContent(JsonConvert.SerializeObject(query), Encoding.UTF8, "application/json");
                var response = await _client.PostAsync("machine-dev-db/_find", content, cancellationToken);
                if (!response.IsSuccessStatusCode)
                {
                    throw new NotImplementedException();
                }
                var jsonString = await response.Content.ReadAsStringAsync(cancellationToken);
                return JsonConvert.DeserializeObject<FindDataResponse>(jsonString);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private async Task<Doc> FindDocByHostnameAsync(string hostname, CancellationToken cancellationToken = default)
        {
            var query = new
            {
                selector = new
                {
                    Machine = new
                    {
                        Hostname = hostname.ToLower()
                    }
                }
            };

            var content = new StringContent(JsonConvert.SerializeObject(query), Encoding.UTF8, "application/json");
            var response = await _client.PostAsync("machine-dev-db/_find", content, cancellationToken);
            if (!response.IsSuccessStatusCode)
            {
                throw new NotImplementedException();
            }
            var jsonString = await response.Content.ReadAsStringAsync(cancellationToken);
            var findResponse = JsonConvert.DeserializeObject<FindDataResponse>(jsonString)
                ?? throw new NotImplementedException();

            return findResponse.Docs.FirstOrDefault() ?? throw new Exception("Não encontrado nenhum documento");
        }

        private async Task SendToDatabase(StringContent content, string? docId = null, CancellationToken cancellationToken = default)
        {
            var response = await _client.PutAsync($"machine-dev-db/{docId}", content, cancellationToken);
            if (!response.IsSuccessStatusCode)
                throw new NotImplementedException();
            return;
        }
    }
}
