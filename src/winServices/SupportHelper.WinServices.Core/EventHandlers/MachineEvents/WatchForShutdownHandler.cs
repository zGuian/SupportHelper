using Microsoft.AspNetCore.SignalR.Client;
using SupportHelper.Communication.Responses;
using SupportHelper.WinServices.Application.Interfaces.Events;
using SupportHelper.WinServices.Core.Models;
using SupportHelper.WinServices.Core.Models.ValueObjects;
using System.Management;
using System.Runtime.Versioning;

namespace SupportHelper.WinServices.Core.EventHandlers.MachineEvents
{
    public sealed class WatchForShutdownHandler : IWatchForShutdownHandler
    {
        [SupportedOSPlatform("windows")]
        private ManagementEventWatcher? _shutdownEventWatcher;

        [SupportedOSPlatform("windows")]
        public void Run(HubConnection connection, CancellationToken cancellationToken)
        {
            try
            {
                MachineModel machine = MachineModel.Create();
                var response = ResponseStatusMachineJson.Create(false, machine.Hostname, false, machine.CurrentUsername,
                    machine.DomainName, machine.OperationalSystem, ConvertToResponse(machine.NetworkBoards));

                string query = "SELECT * FROM Win32_ComputerShutdown";
                _shutdownEventWatcher = new ManagementEventWatcher(query);
                _shutdownEventWatcher.EventArrived += async (sender, e) =>
                {
                    await connection.SendAsync("MachineDisconnected", response, cancellationToken);
                };
                _shutdownEventWatcher.Start();
            }
            catch (Exception ex)
            {
                throw;
            }
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
