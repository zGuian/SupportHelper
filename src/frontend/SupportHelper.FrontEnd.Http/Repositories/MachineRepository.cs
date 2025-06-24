using Microsoft.Extensions.Logging;
using SupportHelper.Communication.Responses;
using System.Net.Http.Headers;
using System.Text.Json;

namespace SupportHelper.FrontEnd.Http.Repositories
{
    public class MachineRepository
    {
        private readonly ILogger<MachineRepository> _logger;
        private readonly HttpClient _client;


        public MachineRepository(ILogger<MachineRepository> logger, IHttpClientFactory factory)
        {
            _logger = logger;
            _client = factory.CreateClient("Api_SupportHelper");
        }

        public async Task<bool> RequestStatusToMachineAsync(string hostname)
        {
            try
            {
                HttpResponseMessage response = await _client.GetAsync($"StatusMachine/{hostname}");
                if (!response.IsSuccessStatusCode)
                {
                    string message = await response.Content.ReadAsStringAsync();
                    throw new Exception(message);
                }
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError("MESSAGEM DE RETORNO: {message}", ex.Message);
                return false;
            }
        }

        public async Task UpdateSgpClientAsync()
        {
            try
            {
                MediaTypeHeaderValue mediaType = new("application/json");
                string json = JsonSerializer.Serialize("");
                StringContent content = new(json, mediaType);
                HttpResponseMessage response = await _client.PostAsync("", content);
                if (!response.IsSuccessStatusCode && response.Content is null)
                {
                    throw new Exception("ERROR");
                }
                json = await response.Content.ReadAsStringAsync();
                ResponseUpdateSgpClientJson obj = JsonSerializer.Deserialize<ResponseUpdateSgpClientJson>(json)
                    ?? throw new Exception();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
