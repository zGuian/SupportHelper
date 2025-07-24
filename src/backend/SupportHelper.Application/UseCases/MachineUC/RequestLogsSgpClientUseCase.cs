using Microsoft.Extensions.Logging;
using SupportHelper.Application.Interfaces;
using SupportHelper.Communication.Requests;
using SupportHelper.Domain.Interfaces.MQServices;
using SupportHelper.Domain.Interfaces.Repositories.Database;
using SupportHelper.Domain.Interfaces.SignalRContext;
using SupportHelper.Exceptions.ExceptionsBase;

namespace SupportHelper.Application.UseCases.MachineUC
{
    public class RequestLogsSgpClientUseCase : IRequestLogsSgpClientUseCase
    {
        private readonly ILogger<RequestLogsSgpClientUseCase> _logger;
        private readonly IMachineSignalRServices _machineSignalR;
        private readonly IMachineRepository _machineRepository;

        public RequestLogsSgpClientUseCase(ILogger<RequestLogsSgpClientUseCase> logger, IMachineSignalRServices machineSignalR, IMachineRepository machineRepository)
        {
            _logger = logger;
            _machineSignalR = machineSignalR;
            _machineRepository = machineRepository;
        }

        public async Task ExecuteAsync(RequestLogsSgpClientJson request, CancellationToken cancellationToken = default)
        {
            var connId = await _machineRepository.GetConnectionByHostnameAsync(request.Hostname);
            var response = await _machineSignalR.GetLogSgpClientAsync(request, connId, cancellationToken);
            if (response.StartsWith("NOK -"))
            {
                throw new GenericErrorException([response]);
            }
            return;
        }
    }
}
