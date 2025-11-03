using Microsoft.AspNetCore.SignalR.Client;
using SupportHelper.Service.Domain.DTOs.Requests;
using SupportHelper.Service.Domain.Interface.EventHandler;
using SupportHelper.Service.Domain.Interface.Services;
using System.Text.Json;

namespace SupportHelper.Service.Domain.EventsHandlers
{
    public class UpdateSgpClientHandler : ISignalREventHandler
    {
        private readonly IMachineServices _machineService;

        public UpdateSgpClientHandler(IMachineServices machineService)
        {
            _machineService = machineService;
        }

        public void On(HubConnection connection, CancellationToken stoppingToken)
        {
            connection.On<string, RequestUpdateSgpClient>("UpdateSgpClient", async (receivedRequestId, request) =>
            {
                var value = await _machineService.UpdateSgpClient(request.ProductionLine);
                var response = JsonSerializer.Serialize(value);
                await connection.InvokeAsync("ResponseStatusAsync", receivedRequestId, response);
            });
        }
    }
}
