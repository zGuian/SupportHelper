using Microsoft.AspNetCore.SignalR.Client;
using SupportHelper.Communication.Responses;
using SupportHelper.WinServices.Application.Interfaces.Services;
using SupportHelper.WinServices.Core.Models;
using SupportHelper.WinServices.Core.Models.ValueObjects;

namespace SupportHelper.WinServices.Core.Workers
{
    public class SignalRWorker : BackgroundService
    {
        private HubConnection? _connection;
        private readonly ILogger<SignalRWorker> _logger;
        private readonly IMachineService _machineService;

        public SignalRWorker(ILogger<SignalRWorker> logger, IMachineService machineService)
        {
            _logger = logger;
            _machineService = machineService;
        }

        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _connection = new HubConnectionBuilder()
                .WithUrl($"http://localhost:5001/SupportHelperConnectionSignalR?hostname={Environment.MachineName}")
                .WithAutomaticReconnect()
                .Build();

            _connection.On("RequestStatusToMachine", async () =>
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

                await _connection.InvokeAsync("ResponseStatusToMachine", response);
                _logger.LogInformation("Resposta enviada com sucesso");
            });

            _connection.Reconnecting += error =>
            {
                _logger.LogWarning("Tentando reconectar ao SignalR...");
                return Task.CompletedTask;
            };

            _connection.Reconnected += connectionId =>
            {
                _logger.LogInformation("Reconectado com SignalR! ConnectionId: {id}", connectionId);
                return Task.CompletedTask;
            };

            _connection.Closed += async error =>
            {
                _logger.LogWarning("Conexão com SignalR foi encerrada. Tentando reconectar em 5 segundos...");
                await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
                await _connection.StartAsync(stoppingToken);
            };

            try
            {
                await _connection.StartAsync(stoppingToken);
                _logger.LogInformation("Conectado ao SignalR. ConnectionId: {id}", _connection.ConnectionId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao conectar com o SignalR");
            }

            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(1000, stoppingToken);
            }

            throw new Exception();
        }
    }
}
