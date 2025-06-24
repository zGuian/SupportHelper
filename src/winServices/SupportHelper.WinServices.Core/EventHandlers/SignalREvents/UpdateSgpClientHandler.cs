using Microsoft.AspNetCore.SignalR.Client;
using SupportHelper.WinServices.Application.Interfaces.Events;
using SupportHelper.WinServices.Application.Interfaces.Services;

namespace SupportHelper.WinServices.Core.EventHandlers.SignalREvents
{
    public class UpdateSgpClientHandler : ISignalREventHandler
    {
        private readonly IMachineService _machineService;

        public UpdateSgpClientHandler(IMachineService machineService)
        {
            _machineService = machineService;
        }

        public void Register(HubConnection connection, CancellationToken stoppingToken)
        {
            connection.On("UpdateSgpClient", async (string productionLine) => 
            {
                await _machineService.UpdateSgpClient(productionLine);
            });
        }
    }
}
