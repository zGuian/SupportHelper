using SupportHelper.Application.Interfaces;
using SupportHelper.Communication.Requests;
using SupportHelper.Communication.Responses;
using SupportHelper.Domain.Interfaces.Repositories.Database;
using SupportHelper.Domain.Interfaces.Repositories.Memory;
using SupportHelper.Domain.Interfaces.SignalRContext;

namespace SupportHelper.Application.UseCases.MachineUC
{
    public sealed class RequestUpdateSgpClientUseCase : IRequestUpdateSgpClientUseCase
    {
        private readonly IMachineSignalRServices _signalRService;
        private readonly IMachineRepository _machineRepository;

        public RequestUpdateSgpClientUseCase(IMachineSignalRServices signalRService, IMachineRepository machineRepository)
        {
            _signalRService = signalRService;
            _machineRepository = machineRepository;
        }

        public async Task<ResponseUpdateSgpClientJson> ExecuteAsync(RequestUpdateSgpClientJson requestJson)
        {
            var connectionId = _machineRepository.GetConnectionByHostnameAsync(requestJson.Hostname);
            ResponseUpdateSgpClientJson response = await _signalRService.UpdateSgpClientAsync(requestJson.Hostname, 
                requestJson.ProductionLine);
            return response;
        }
    }
}
