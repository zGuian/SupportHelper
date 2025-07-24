using Microsoft.AspNetCore.SignalR.Client;
using SupportHelper.WinServices.Application.Interfaces.Events;
using SupportHelper.WinServices.Application.Interfaces.Services;

namespace SupportHelper.WinServices.Core.EventHandlers.SignalREvents
{
    public class RequestLogSgpClientHandler : ISignalREventHandler
    {
        private readonly IMachineService _machineService;

        public RequestLogSgpClientHandler(IMachineService machineService)
        {
            _machineService = machineService;
        }

        public void Register(HubConnection connection, CancellationToken stoppingToken)
        {
            connection.On("GetLogSgpClient", async (string productionLine, string requestId) =>
            {
                await _machineService.MakeAvailableLogSgpClient(productionLine, requestId);
            });
        }
    }
}
