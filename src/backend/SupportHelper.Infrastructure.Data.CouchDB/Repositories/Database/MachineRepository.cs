using SupportHelper.Communication.Dtos.CouchDbDto;
using SupportHelper.Domain.Aggregates;
using SupportHelper.Domain.Interfaces.Repositories.Database;
using SupportHelper.Domain.Interfaces.Repositories.Memory;
using SupportHelper.Exceptions.ExceptionsBase;
using SupportHelper.Infrastructure.Data.CouchDB.Json.Responses;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace SupportHelper.Infrastructure.Data.CouchDB.Repositories.Database
{
    public sealed class MachineRepository : IMachineRepository
    {
        private readonly ITokenMemoryRepository _tokenMemoryRepository;
        private readonly IConnectionMemoryRepository _connectionMemoryRepository;
        private readonly HttpClient _client;

        public MachineRepository(IHttpClientFactory clientFactory, ITokenMemoryRepository tokenMemoryRepository,
            IConnectionMemoryRepository connectionMemoryRepository)
        {
            _tokenMemoryRepository = tokenMemoryRepository;
            _connectionMemoryRepository = connectionMemoryRepository;
            _client = clientFactory.CreateClient("CouchDB");
        }

        public async Task<AllDocsDto> GetAllAsync(int limit, int skip)
        {
            try
            {
                HttpResponseMessage response = await _client.GetAsync("_all_docs");
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

        public async Task<BaseDto> GetByHostnameAsync(string hostname)
        {
            try
            {
                HttpRequestMessage httpRequest = new(HttpMethod.Post, $"");
                httpRequest.Headers.Authorization = new AuthenticationHeaderValue("AuthSession", "");
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
                var response = await _client.PostAsync("_find", content);
                if (!response.IsSuccessStatusCode)
                {
                    throw new NotImplementedException();
                }
                var baseDto = JsonSerializer.Deserialize<BaseDto>(
                    await response.Content.ReadAsStreamAsync()) ?? throw new Exception();
                return baseDto;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task InsertAsync(MachineSchemaJson schema)
        {
            try
            {
                HttpRequestMessage httpRequest = new(HttpMethod.Post, "");
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

        public async Task InsertAsync(string hostname, string connId)
        {
            try
            {
                var json = new
                {
                    Machine = new
                    {
                        Hostname = hostname
                    },
                    SignalR = new
                    {
                        ConnectionId = connId
                    }
                };

                var content = new StringContent(JsonSerializer.Serialize(json),
                    Encoding.UTF8, "application/json");

                var request = new HttpRequestMessage(HttpMethod.Post, "");
                request.Content = content;

                var response = await _client.SendAsync(request);
                if (!response.IsSuccessStatusCode) 
                {

                }
                var data = await JsonSerializer.DeserializeAsync<InsertDataResponse>(
                    await response.Content.ReadAsStreamAsync());

                if (data.Ok)
                    await Task.CompletedTask;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<ResponseBaseDto> InsertOrUpdateAsync(MachineSchemaJson schema)
        {
            try
            {
                HttpRequestMessage httpRequest = new(HttpMethod.Put, $"{schema.Machine.Id}");
                string json = JsonSerializer.Serialize(schema);
                httpRequest.Content = new StringContent(json, Encoding.UTF8, "application/json");
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
    }
}
