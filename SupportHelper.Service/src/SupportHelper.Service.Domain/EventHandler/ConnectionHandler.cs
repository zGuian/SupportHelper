using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Logging;
using SupportHelper.Service.Domain.Interface.EventHandler;
using SupportHelper.Service.Domain.Interface.Services;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace SupportHelper.Service.Domain.EventHandler
{
    public class ConnectionHandler(ILogger<ConnectionHandler> logger
        , IMachineServices services) : ISignalREventHandler
    {
        private readonly ILogger<ConnectionHandler> _logger = logger;
        private readonly IMachineServices _machineServices = services;

        public void Register(HubConnection connection, CancellationToken stoppingToken)
        {
            connection.Reconnecting += error =>
            {
                _logger.LogWarning("Tentando reconectar ao SignalR...");
                return Task.CompletedTask;
            };

            connection.Reconnected += connectionId =>
            {
                _logger.LogInformation("Reconectado com SignalR! ConnectionId: {id}", connectionId);
                return Task.CompletedTask;
            };

            connection.Closed += async error =>
            {
                _logger.LogWarning("Conexão com SignalR foi encerrada. Tentando reconectar em 40 segundos...");
                await Task.Delay(TimeSpan.FromSeconds(40), stoppingToken);
                await connection.StartAsync();
            };
        }
    }
}
