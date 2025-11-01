using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Logging;
using SupportHelper.Service.Domain.Interface.EventHandler;
using SupportHelper.Service.Domain.Interface.UseCases;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace SupportHelper.Service.Domain.EventHandler
{
    public class RequestStatusToMachineHandler(IGetStatusMachineUseCase useCase
        , ILogger<RequestStatusToMachineHandler> logger) : ISignalREventHandler
    {
        private readonly IGetStatusMachineUseCase _useCase = useCase;
        private readonly ILogger<RequestStatusToMachineHandler> _logger = logger;

        public void Register(HubConnection connection, CancellationToken stoppingToken)
        {
            connection.On<string>("StatusMachine", async (receivedRequestId) =>
            {
                var jsonString = await Task.Run(() =>
                {
                    var response = _useCase.Execute();
                    return JsonSerializer.Serialize(response);
                });

                var node = JsonNode.Parse(jsonString) ?? throw new Exception();
                node["connId"] = connection.ConnectionId;
                node["isActive"] = true;
                jsonString = JsonSerializer.Serialize(node);

                await connection.InvokeAsync("ResponseBase", receivedRequestId, jsonString);
                _logger.LogInformation("Resposta enviada com sucesso");
            });
        }
    }
}
