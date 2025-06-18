using Microsoft.AspNetCore.SignalR.Client;
using SupportHelper.WinServices.Application.Interfaces.Events;

namespace SupportHelper.WinServices.Core.EventHandlers.SignalREvents
{
    public class ConnectionHandler : ISignalREventHandler
    {
        private readonly ILogger<ConnectionHandler> _logger;

        public ConnectionHandler(ILogger<ConnectionHandler> logger)
        {
            _logger = logger;
        }

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
                _logger.LogWarning("Conexão com SignalR foi encerrada. Tentando reconectar em 5 segundos...");
                await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
                await connection.StartAsync(stoppingToken);
            };
        }
    }
}
