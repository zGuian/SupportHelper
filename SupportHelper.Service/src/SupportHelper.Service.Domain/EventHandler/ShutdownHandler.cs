using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Logging;
using SupportHelper.Service.Domain.Events;
using SupportHelper.Service.Domain.Interface.EventHandler;
using SupportHelper.Service.Domain.Interface.UseCases;

namespace SupportHelper.Service.Domain.EventHandler
{
    public class ShutdownHandler(ILogger<ShutdownHandler> logger
        , IGetStatusMachineUseCase getStatusMachine) : IMachineEventHandler
    {
        private readonly ILogger<ShutdownHandler> _logger = logger;
        private readonly IGetStatusMachineUseCase _getStatusMachineUseCase = getStatusMachine;

        public void Register(MachineEvents machineEvents, HubConnection hubConnection)
        {
            machineEvents.OnShutdown += async () =>
            {
                var hostname = await Task.Run(() =>
                {
                    var machine = _getStatusMachineUseCase.Execute();
                    return machine.Hostname;
                });

                try
                {
                    await hubConnection.SendAsync("ClientHasShutdown", hostname);
                }
                catch (Exception ex)
                {
                    _logger.LogWarning("Não foi possivel enviar comando de desligamento não há conexão com servidor.");
                    _logger.LogWarning($"{ex.Message}");
                }
            };
        }
    }
}
