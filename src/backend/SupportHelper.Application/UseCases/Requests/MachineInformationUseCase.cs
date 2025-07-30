using Microsoft.Extensions.Logging;
using SupportHelper.Application.Interfaces;
using SupportHelper.Communication.Requests;
using SupportHelper.Domain.Interfaces.SignalRContext;

namespace SupportHelper.Application.UseCases.Requests
{
    public class MachineInformationUseCase : IMachineInformationUseCase
    {
        private readonly ILogger<MachineInformationUseCase> _logger;
        private readonly IMachineSignalRServices _machineSignalR;

        public MachineInformationUseCase(ILogger<MachineInformationUseCase> logger, IMachineSignalRServices machineSignalR)
        {
            _logger = logger;
            _machineSignalR = machineSignalR;
        }

        [Obsolete("Este método foi removido e não deve ser usado. Causará um erro ao executar.", true)]
        public async Task ExecuteAsync(RequestStatusMachineJson request, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("ENVIADO SINAL VIA SIGNALR");
            var response = await _machineSignalR.RequestStatusAsync(request.Hostname, cancellationToken);
        }
    }
}