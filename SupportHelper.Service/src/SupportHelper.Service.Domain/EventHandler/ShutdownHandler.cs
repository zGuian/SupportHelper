using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Logging;
using SupportHelper.Service.Domain.Interface.EventHandler;
using SupportHelper.Service.Domain.Interface.UseCases;
using System.Threading.Tasks;

namespace SupportHelper.Service.Domain.EventHandler
{
    public delegate void MachineShutdownEventHandler();

    public class ShutdownHandler(ILogger<ShutdownHandler> logger
        , IGetStatusMachineUseCase getStatusMachine)
    {
        private readonly ILogger<ShutdownHandler> _logger = logger;
        private readonly IGetStatusMachineUseCase _getStatusMachineUseCase = getStatusMachine;
        private event MachineShutdownEventHandler? MachineShutdown;

        public void MachhineShutdown(HubConnection connection, CancellationToken stoppingToken)
        {
            MachineShutdown?.Invoke();
        }
    }
}
