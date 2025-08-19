using Microsoft.AspNetCore.SignalR.Client;
using SupportHelper.Communication.Responses;
using SupportHelper.WinServices.Application.Interfaces.Events;
using SupportHelper.WinServices.Core.Converters;
using SupportHelper.WinServices.Core.Models;
using System.Management;
using System.Runtime.Versioning;
using System.Text.Json;

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
                
                string query = "SELECT * FROM Win32_ComputerShutdown";
                _shutdownEventWatcher = new ManagementEventWatcher(query);
                _shutdownEventWatcher.EventArrived += async (sender, e) =>
                {
                    var response = ResponseStatusMachineJson.Create(machine.Id, false, machine.Hostname, false, machine.CurrentUsername,
                    machine.DomainName, machine.OperationalSystem,
                        NetworkBoardConvert.EntityToResponse(machine.NetworkBoards), machine.UpTime, machine.LastUpdate);
                    var json = JsonSerializer.Serialize(response);
                    await connection.SendAsync("ClientHasShutdown", json, cancellationToken);
                };
                _shutdownEventWatcher.Start();
                _logger.LogInformation("Iniciado monitoração do evento: [Shutdown]");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocorreu um erro inesperado: {ex.Message}");
            }
        }

        public void Stop(CancellationToken cancellationToken)
        {
            _shutdownEventWatcher?.Stop();
            _shutdownEventWatcher?.Dispose();
        }
    }
}
