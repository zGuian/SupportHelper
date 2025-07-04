using Microsoft.AspNetCore.SignalR.Client;
using SupportHelper.Communication.Requests;
using SupportHelper.Communication.Responses;
using SupportHelper.WinServices.Application.Interfaces.Events;
using SupportHelper.WinServices.Core.Converters;
using SupportHelper.WinServices.Core.Models;
using SupportHelper.WinServices.Core.Models.ValueObjects;
using System.Diagnostics;

namespace SupportHelper.WinServices.Core.EventHandlers.SignalREvents
{
    public sealed class RequestStatusToMachineHandler : ISignalREventHandler
    {
        private readonly ILogger<RequestStatusToMachineHandler> _logger;

        public RequestStatusToMachineHandler(ILogger<RequestStatusToMachineHandler> logger)
        {
            _logger = logger;
        }

        public void Register(HubConnection connection, CancellationToken stoppingToken)
        {
            connection.On<RequestStatusMachineJson, ResponseStatusMachineJson>("RequestStatusMachine", request =>
            {
                MachineModel machine = MachineModel.Create();
                var response = new ResponseStatusMachineJson
                {
                    IsConnected = true,
                    Hostname = machine.Hostname,
                    SgpIsRunning = VerifiySgpIsRunning(),
                    CurrentUsername = machine.CurrentUsername,
                    DomainName = machine.DomainName,
                    OperationalSystem = machine.OperationalSystem,
                    NetworkBoards = NetworkBoardConvert.EntityToResponse(machine.NetworkBoards),
                };

                _logger.LogInformation("Resposta enviada com sucesso");
                return response;
            });
        }

        private static bool VerifiySgpIsRunning()
        {
            Process[] processes = Process.GetProcesses();

            foreach (Process process in processes)
            {
                if (process.ProcessName.StartsWith("DCX.ITLC"))
                {
                    return true;
                }
                continue;
            }
            return false;
        }
    }
}
