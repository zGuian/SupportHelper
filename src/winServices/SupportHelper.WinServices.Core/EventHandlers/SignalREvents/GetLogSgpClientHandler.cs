using Microsoft.AspNetCore.SignalR.Client;
using SupportHelper.WinServices.Application.Interfaces.Events;
using SupportHelper.WinServices.Application.Interfaces.Services;

namespace SupportHelper.WinServices.Core.EventHandlers.SignalREvents
{
    public class GetLogSgpClientHandler : ISignalREventHandler
    {
        private readonly IMachineService _machineService;

        public GetLogSgpClientHandler(IMachineService machineService)
        {
            _machineService = machineService;
        }

        public void Register(HubConnection connection, CancellationToken stoppingToken)
        {
            connection.On("GetLogSgpClient", async (string productionLine) =>
            {
                await _machineService.MakeAvailableLogSgpClient(productionLine);
            });
        }
    }
}
