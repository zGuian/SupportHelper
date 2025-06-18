using Microsoft.AspNetCore.SignalR.Client;
using SupportHelper.Communication.Responses;
using SupportHelper.WinServices.Application.Interfaces.Events;
using SupportHelper.WinServices.Core.Models;
using SupportHelper.WinServices.Core.Models.ValueObjects;

namespace SupportHelper.WinServices.Core.EventHandlers.SignalREvents
{
    public class RequestStatusToMachineHandler : ISignalREventHandler
    {
        private readonly ILogger<RequestStatusToMachineHandler> _logger;

        public RequestStatusToMachineHandler(ILogger<RequestStatusToMachineHandler> logger)
        {
            _logger = logger;
        }

        public void Register(HubConnection connection, CancellationToken stoppingToken)
        {
            connection.On("RequestStatusToMachine", async () =>
            {
                MachineModel machine = MachineModel.Create();
                List<NetworkBoardResponse> networkBoard = [];
                foreach (NetworkBoard item in machine.NetworkBoards)
                {
                    networkBoard.Add(NetworkBoardResponse.Create(item.Description, item.Ipv4, item.Ipv6,
                        item.MacAddress, item.InUse));
                }

                var response = MachineInformationResponse.Create(machine.Hostname, machine.CurrentUsername, machine.DomainName,
                    machine.OperationalSystem, [.. networkBoard]);

                await connection.InvokeAsync("ResponseStatusToMachine", response);
                _logger.LogInformation("Resposta enviada com sucesso");
            });
        }
    }
}
