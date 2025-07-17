using SupportHelper.Communication.Dtos.CouchDbDto;
using SupportHelper.Domain.Aggregates;
using SupportHelper.Domain.Interfaces.Repositories.Database;
using SupportHelper.Domain.Interfaces.Repositories.Memory;
using SupportHelper.Exceptions.ExceptionsBase;
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

        public async Task<BaseDto> GetByIdAsync(string id)
        {
            try
            {
                HttpRequestMessage httpRequest = new(HttpMethod.Get, $"");
                httpRequest.Headers.Authorization = new AuthenticationHeaderValue("AuthSession", "");
                HttpResponseMessage response = await _client.SendAsync(httpRequest);
                if (!response.IsSuccessStatusCode)
                {
                    throw new GenericErrorException(["OCORREU UM ERRO GENERICO"]);
                }
                Stream stream = await response.Content.ReadAsStreamAsync();
                return await JsonSerializer.DeserializeAsync<BaseDto>(stream)
                    ?? throw new GenericErrorException(["OCORREU UM ERRO GENERICO"]);
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
