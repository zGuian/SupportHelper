using Microsoft.AspNetCore.SignalR.Client;
using SupportHelper.Communication.Requests;
using SupportHelper.Communication.Responses;
using SupportHelper.WinServices.Application.Interfaces.Events;
using SupportHelper.WinServices.Application.Interfaces.Services;
using System.Text.Json;

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
            connection.On<string,RequestUpdateSgpClientJson>("UpdateSgpClient", async (receivedRequestId, request) =>
            {
                var value = await _machineService.UpdateSgpClient(request.ProductionLine);
                var response = JsonSerializer.Serialize(value);
                await connection.InvokeAsync("ResponseStatusAsync", receivedRequestId, response);
            });
        }
    }
}
