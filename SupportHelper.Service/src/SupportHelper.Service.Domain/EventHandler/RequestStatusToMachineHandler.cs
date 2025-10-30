using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Logging;
using SupportHelper.Service.Domain.Interface.EventHandler;
using SupportHelper.Service.Domain.Interface.Services;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace SupportHelper.Service.Domain.EventHandler
{
    public class RequestStatusToMachineHandler(IMachineServices services
        , ILogger<RequestStatusToMachineHandler> logger) : ISignalREventHandler
    {
        private readonly IMachineServices _services = services;
        private readonly ILogger<RequestStatusToMachineHandler> _logger = logger;

        public void Register(HubConnection connection, CancellationToken stoppingToken)
        {
            connection.On<string>("StatusMachine", async (receivedRequestId) =>
            {
                var response = await _services.GetStatusToMachine();
                var node = JsonNode.Parse(response) ?? throw new Exception();
                node["connId"] = connection.ConnectionId;
                node["isActive"] = true;
                response = JsonSerializer.Serialize(node);

                await connection.InvokeAsync("ResponseBase", receivedRequestId, response);
                _logger.LogInformation("Resposta enviada com sucesso");
            });
        }
    }
}
