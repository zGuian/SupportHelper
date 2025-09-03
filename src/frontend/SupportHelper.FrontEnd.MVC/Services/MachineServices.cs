using Newtonsoft.Json;
using SupportHelper.FrontEnd.MVC.DTOs;
using SupportHelper.FrontEnd.MVC.Interfaces;
using SupportHelper.FrontEnd.MVC.Models;

namespace SupportHelper.FrontEnd.MVC.Services
{
    public class MachineServices : IMachineServices
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<MachineServices> _logger;

        public MachineServices(ILogger<MachineServices> logger, IHttpClientFactory httpClient)
        {
            _logger = logger;
            _httpClient = httpClient.CreateClient("Default");
        }

        public async Task<ResponseBase<IEnumerable<MachineModel>>> GetMachineAsync(CancellationToken ct = default)
        {
            try
            {
                var response = await _httpClient.GetAsync("Machine/GetMachineConnected", ct);
                if (!response.IsSuccessStatusCode)
                {
                    return ResponseBase<IEnumerable<MachineModel>>.ReturnFalse("Não foi possivel fazer requisição para a API. tente novamente");
                }
                var jsonString = await response.Content.ReadAsStringAsync(ct);
                var obj = JsonConvert.DeserializeObject<JsonBaseDto<IEnumerable<MachineModel>>>(jsonString);
                if (obj != null && obj.Data != null)
                {
                    return ResponseBase<IEnumerable<MachineModel>>.ReturnSuccess(obj.Data);
                }
                throw new Exception("Não foi encontrado nenhuma informação.");
            }
            catch (Exception ex)
            {
                _logger.LogError("Houve um erro não rastreado");
                return ResponseBase<IEnumerable<MachineModel>>.ReturnFalse(ex.Message);
            }
        }

        public async Task<ResponseBase<MachineModel>> GetMachineAsync(string hostname, CancellationToken ct = default)
        {
            try
            {
                var response = await _httpClient.GetAsync(hostname, ct);
                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception();
                }
                var jsonString = await response.Content.ReadAsStringAsync(ct);
                var obj = JsonConvert.DeserializeObject<JsonBaseDto<MachineModel>>(jsonString);
                if (obj != null && obj.Data != null)
                {
                    return ResponseBase<MachineModel>.ReturnSuccess(obj.Data);
                }
                return ResponseBase<MachineModel>.ReturnFalse(string.Empty);
            }
            catch (Exception ex)
            {
                _logger.LogError("Houve um erro não rastreado");
                return ResponseBase<MachineModel>.ReturnFalse(ex.Message);
            }
        }
    }
}
