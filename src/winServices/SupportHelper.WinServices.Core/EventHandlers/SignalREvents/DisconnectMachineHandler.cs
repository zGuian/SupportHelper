using Microsoft.AspNetCore.SignalR.Client;
using SupportHelper.Communication.Responses;
using SupportHelper.WinServices.Application.Interfaces.Events;
using SupportHelper.WinServices.Core.Models;
using SupportHelper.WinServices.Core.Models.ValueObjects;

namespace SupportHelper.WinServices.Core.EventHandlers.SignalREvents
{
    public sealed class DisconnectMachineHandler
    {
        public void Register(HubConnection connection, CancellationToken stoppingToken)
        {
            MachineModel machine = MachineModel.Create();
            var response = ResponseStatusMachineJson.Create(machine.Id, false, machine.Hostname, false, machine.CurrentUsername,
                machine.DomainName, machine.OperationalSystem, ConvertToResponse(machine.NetworkBoards.ToArray()), machine.UpTime, 
                machine.LastUpdate);
            Task.Run(async () =>
            {
                await connection.SendAsync("MachineDisconnect", response, stoppingToken);
            }, stoppingToken);
        }

        private static NetworkBoardResponse[] ConvertToResponse(NetworkBoard[] networkBoard)
        {
            List<NetworkBoardResponse> response = [];
            foreach (var item in networkBoard)
            {
                response.Add(NetworkBoardResponse.Create(item.Description, item.Ipv4, item.Ipv6, item.MacAddress,
                    item.InUse));
            }
            return [.. response];
        }


    }
}
