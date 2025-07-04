using Microsoft.AspNetCore.SignalR.Client;
using SupportHelper.Communication.Responses;
using SupportHelper.WinServices.Application.Interfaces.Events;
using SupportHelper.WinServices.Core.Converters;
using SupportHelper.WinServices.Core.Models;
using System.Management;
using System.Runtime.Versioning;

namespace SupportHelper.WinServices.Core.EventHandlers.MachineEvents
{
    [SupportedOSPlatform("windows")]
    public sealed class WatchForShutdownHandler : IWatchForShutdownHandler
    {
        private ManagementEventWatcher? _shutdownEventWatcher;
        private readonly ILogger<WatchForShutdownHandler> _logger;

        public WatchForShutdownHandler(ILogger<WatchForShutdownHandler> logger)
        {
            _logger = logger;
        }

        public void Run(HubConnection connection, CancellationToken cancellationToken)
        {
            try
            {
                MachineModel machine = MachineModel.Create();
                var response = ResponseStatusMachineJson.Create(false, machine.Hostname, false, machine.CurrentUsername,
                    machine.DomainName, machine.OperationalSystem, NetworkBoardConvert.EntityToResponse(machine.NetworkBoards));

                string query = "SELECT * FROM Win32_ComputerShutdown";
                _shutdownEventWatcher = new ManagementEventWatcher(query);
                _shutdownEventWatcher.EventArrived += async (sender, e) =>
                {
                    await connection.SendAsync("ClientHasShutdown", response, cancellationToken);
                };
                _shutdownEventWatcher.Start();
                _logger.LogInformation("Iniciado monitoração do evento: [Shutdown]");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocorreu um erro inesperado: {ex.Message}");
            }
        }
    }
}
