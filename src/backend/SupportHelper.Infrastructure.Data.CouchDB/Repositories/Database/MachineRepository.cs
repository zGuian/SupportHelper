using Microsoft.Extensions.Logging;
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
                var countTask = _client.GetAsync("machine-dv-db/", cancellationToken);
                var result = await Task.WhenAll(responseTask, countTask);
                var response = await responseTask;
                var countObj = await countTask;
                if (!response.IsSuccessStatusCode && !countObj.IsSuccessStatusCode)
                {
                    throw new GenericErrorException(["HOUVE UMA RESPOSTA HTTP NEGATIVA"]);
                }
                using var doc = JsonDocument.Parse(await countObj.Content.ReadAsByteArrayAsync(cancellationToken));
                var count = doc.RootElement.GetProperty("doc_count").GetInt32();

                Stream stream = await response.Content.ReadAsStreamAsync(cancellationToken);
                return (await JsonSerializer.DeserializeAsync<AllDocsDto>(stream, cancellationToken: cancellationToken)
                    ?? throw new JsonException("Ocorreu um problema ao deserializar objeto"), count);
            }
            catch (Exception ex)
            {
                _logger.LogError("ERROR: {ex}", ex.Message);
                throw;
            }
        }

        public async Task<MachineSchemaJson> GetByHostnameAsync(string hostname, CancellationToken cancellationToken = default)
        {
            var findData = await FindByHostnameAsync(hostname, cancellationToken);
            if (findData == null || findData.Docs == null)
            {
                throw new Exception();
            }
            var doc = findData.Docs.FirstOrDefault() ?? throw new Exception();
            return MachineSchemaJson.Create(doc.Machine, doc.SignalR);
        }

        public async Task<string> GetConnectionByHostnameAsync(string hostname, CancellationToken cancellationToken = default)
        {
            var doc = await FindDocByHostnameAsync(hostname.ToLower(), cancellationToken);
            var connId = doc.SignalR.ConnectionId;
            return connId;
        }

        public async Task InsertAsync(MachineSchemaJson schema, CancellationToken cancellationToken = default)
        {
            try
            {
                HttpRequestMessage httpRequest = new(HttpMethod.Put, $"{schema.Machine.Id}");
                httpRequest.Headers.Authorization = new AuthenticationHeaderValue("AuthSession", "");
                string json = JsonSerializer.Serialize(schema);
                httpRequest.Content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await _client.SendAsync(httpRequest, cancellationToken);
                if (!response.IsSuccessStatusCode)
                {
                    throw new GenericErrorException(["OCORREU UM ERRO GENERICO"]);
                }
                Stream stream = await response.Content.ReadAsStreamAsync(cancellationToken);
                ResponseBaseDto content =
                    await JsonSerializer.DeserializeAsync<ResponseBaseDto>(stream, cancellationToken: cancellationToken)
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
                string json = JsonSerializer.Serialize(doc);
                HttpRequestMessage httpRequest = new(HttpMethod.Put, $"machine-dev-db/{doc.Id}")
                {
                    Content = new StringContent(json, Encoding.UTF8, "application/json")
                };

                HttpResponseMessage response = await _client.SendAsync(httpRequest, cancellationToken);
                if (!response.IsSuccessStatusCode)
                {
                    throw new GenericErrorException(["OCORREU UM ERRO GENERICO"]);
                }
                Stream stream = await response.Content.ReadAsStreamAsync(cancellationToken);
                return await JsonSerializer.DeserializeAsync<ResponseBaseDto>(stream, cancellationToken: cancellationToken)
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
                    doc.SignalR = new SignalR(connId, "");
                    var content = new StringContent(JsonSerializer.Serialize(doc), Encoding.UTF8, "application/json");
                    await SendToDatabase(content, doc.Id, cancellationToken);
                    return;
                }
                var machine = Machine.Create(hostname);
                var signalR = new SignalR(connId, "");
                var machineSchemaJson = MachineSchemaJson.Create(machine, signalR);
                var json = new StringContent(JsonSerializer.Serialize(machineSchemaJson), Encoding.UTF8, "application/json");
                await SendToDatabase(json, machine.Hostname, cancellationToken);
                return;
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
                string json = JsonSerializer.Serialize(schema);
                request.Content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _client.SendAsync(request, cancellationToken);
                if (!response.IsSuccessStatusCode)
                {
                    var stream = await response.Content.ReadAsStreamAsync(cancellationToken);
                    var errorBaseDto = await JsonSerializer.DeserializeAsync<ErrorBaseDto>(stream, cancellationToken: cancellationToken)
                        ?? throw new GenericErrorException(["NÃO FOI POSSIVEL DESERIALIZAR OBJETO"]);
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
                            Hostname = hostname
                        }
                    }
                };

                var content = new StringContent(JsonSerializer.Serialize(query), Encoding.UTF8, "application/json");
                var response = await _client.PostAsync("machine-dev-db/_find", content, cancellationToken);
                if (!response.IsSuccessStatusCode)
                {
                    throw new NotImplementedException();
                }
                using var stream = await response.Content.ReadAsStreamAsync(cancellationToken);
                return await JsonSerializer.DeserializeAsync<FindDataResponse>(stream, cancellationToken: cancellationToken);
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

            var content = new StringContent(JsonSerializer.Serialize(query), Encoding.UTF8, "application/json");
            var response = await _client.PostAsync("machine-dev-db/_find", content, cancellationToken);
            if (!response.IsSuccessStatusCode)
            {
                throw new NotImplementedException();
            }
            var findResponse = await JsonSerializer.DeserializeAsync<FindDataResponse>(
                await response.Content.ReadAsStreamAsync(cancellationToken), cancellationToken: cancellationToken)
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
