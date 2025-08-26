using SupportHelper.Application.Interfaces;
using SupportHelper.Communication.Requests;
using SupportHelper.Communication.Responses;
using SupportHelper.Domain.Interfaces.Repositories.Database;
using SupportHelper.Domain.Interfaces.SignalRContext;

namespace SupportHelper.Application.UseCases.Requests
{
    public sealed class UpdateSgpClientUseCase : IUpdateSgpClientUseCase
    {
        private readonly IMachineSignalRServices _signalRService;
        private readonly IMachineRepository _machineRepository;

        public UpdateSgpClientUseCase(IMachineSignalRServices signalRService, IMachineRepository machineRepository)
        {
            _signalRService = signalRService;
            _machineRepository = machineRepository;
        }

        public async Task<ResponseUpdateSgpClientJson> ExecuteAsync(RequestUpdateSgpClientJson requestJson, CancellationToken cancellationToken = default)
        {
            var connectionId = await _machineRepository.GetConnectionByHostnameAsync(requestJson.Hostname, cancellationToken);
            var response = await _signalRService.UpdateSgpClientAsync(connectionId, requestJson, cancellationToken);
            return response;
        }
    }
}
