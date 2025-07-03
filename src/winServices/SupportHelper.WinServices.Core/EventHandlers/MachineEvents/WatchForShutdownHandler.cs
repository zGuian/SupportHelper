using Microsoft.AspNetCore.SignalR.Client;
using SupportHelper.Communication.Responses;
using SupportHelper.WinServices.Application.Interfaces.Events;
using SupportHelper.WinServices.Core.Converters;
using SupportHelper.WinServices.Core.Models;
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
                    machine.DomainName, machine.OperationalSystem, [.. NetworkBoardConvert.EntityToResponse(machine.NetworkBoards)]);

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
    }
}
