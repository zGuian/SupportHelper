using Microsoft.AspNetCore.SignalR.Client;
using SupportHelper.Communication.Requests;
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
            connection.On("GetLogSgpClient", async (RequestLogsSgpClientJson request, string requestId) =>
            {
                var response = await _machineService.MakeAvailableLogSgpClient(request);
                await connection.SendAsync("ResponseGetLogsSgpClient", requestId, response, stoppingToken);
            });
        }
    }
}
