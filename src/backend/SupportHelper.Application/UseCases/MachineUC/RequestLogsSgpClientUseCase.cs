using Microsoft.Extensions.Logging;
using SupportHelper.Application.Interfaces;
using SupportHelper.Communication.Requests;
using SupportHelper.Communication.Responses;
using SupportHelper.Domain.Interfaces.Repositories.Database;
using SupportHelper.Domain.Interfaces.SignalRContext;
using SupportHelper.Exceptions.ExceptionsBase;
using System.Text.Json;

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
            var connId = await _machineRepository.GetConnectionByHostnameAsync(request.Hostname, cancellationToken);
            var responseBase = await _machineSignalR.GetLogSgpClientAsync(request, connId, cancellationToken);
            if (responseBase.StartsWith("NOK"))
            {
                throw new Exception(responseBase);
            }
            var response = JsonSerializer.Deserialize<ResponseLogsSgpClientJson>(responseBase)
                ?? throw new Exception();
            if (!response.IsSuccess)
            {
                throw new GenericErrorException([response.Message]);
            }
        }
    }
}
