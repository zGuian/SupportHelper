using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Logging;
using SupportHelper.Service.Domain.Interface.UseCases;
using SupportHelper.Service.Domain.Interface.Workers;

namespace SupportHelper.Service.Domain.EventsHandlers
{
    public class MachineShutdownHandler(ILogger<MachineShutdownHandler> logger
        , IGetStatusMachineUseCase getStatusMachine)
    {
        private readonly ILogger<MachineShutdownHandler> _logger = logger;
        private readonly IGetStatusMachineUseCase _getStatusMachineUseCase = getStatusMachine;

        public void On(ISignalRWorker worker, HubConnection hubConnection)
        {
            worker.OnMachineShutdown += async (sender, e) =>
            {
                var hostname = _getStatusMachineUseCase.Execute().Hostname;
                try
                {
                    await hubConnection.SendAsync("ClientHasShutdown", hostname);
                    _logger.LogInformation("Disparado evento para signalR. Metodo: [ClientHasShutdown]");
                }
                catch (Exception ex)
                {
                    _logger.LogWarning("Não foi possivel enviar comando de desligamento não há conexão com servidor.");
                    _logger.LogWarning("{0message}", ex.Message);
                }
            };
        }
    }
}
