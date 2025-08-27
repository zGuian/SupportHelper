using SupportHelper.Communication.Responses;
using SupportHelper.FrontEnd.MVC.Interfaces;
using SupportHelper.FrontEnd.MVC.Models;
using System.Text.Json;

namespace SupportHelper.FrontEnd.MVC.Services
{
    public class MachineServices : IMachineServices
    {
        private readonly HttpClient _client;

        public MachineServices(IHttpClientFactory factory)
        {
            _client = factory.CreateClient();
        }

        public async Task<MachineModel> GetMachineByHostnameAsync(string hostname)
        {
            try
            {
                var response = await _client.GetAsync($"api/v1/Machine/StatusMachine/{hostname}");
                if (!response.IsSuccessStatusCode)
                {

                }
                var jsonString = await response.Content.ReadAsStringAsync();
                var content = JsonSerializer.Deserialize<ResponseMachine>(jsonString)
                    ?? throw new Exception();
                return new MachineModel(content.Id,
                                        content.Hostname,
                                        content.CurrentUsername,
                                        content.DomainName,
                                        content.OperationalSystem,
                                        content.UpTime,
                                        content.LastUpdate);
            }
            catch (Exception)
            {
                throw;
            }

        }
    }
}
