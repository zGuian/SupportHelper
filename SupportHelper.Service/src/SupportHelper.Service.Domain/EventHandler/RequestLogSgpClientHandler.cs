using Microsoft.AspNetCore.SignalR.Client;
using SupportHelper.Service.Domain.DTOs.Requests;
using SupportHelper.Service.Domain.Interface.EventHandler;
using SupportHelper.Service.Domain.Interface.Services;

namespace SupportHelper.Service.Domain.EventHandler
{
    public class RequestLogSgpClientHandler(IMachineServices machineService) : ISignalREventHandler
    {
        private readonly IMachineServices _machineService = machineService;

        public void Register(HubConnection connection, CancellationToken stoppingToken)
        {
            connection.On("GetLogSgpClient", async (RequestLogsSgpClient request, string requestId) =>
            {
                var response = await _machineService.MakeAvailableLogSgpClient(request.ProductionLine, requestId);
                await connection.InvokeAsync("ResponseGetLogsSgpClient", requestId, response, stoppingToken);
                //await connection.SendAsync("ResponseGetLogsSgpClient", requestId, response, stoppingToken);
            });
        }
    }
}
