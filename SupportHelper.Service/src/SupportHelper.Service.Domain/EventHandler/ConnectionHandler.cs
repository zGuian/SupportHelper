using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Logging;
using SupportHelper.Service.Domain.Interface.EventHandler;
using SupportHelper.Service.Domain.Interface.Services;

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
                _logger.LogWarning("Conexão com SignalR foi encerrada. Iniciando processo de reconexão manual...");
                if (stoppingToken.IsCancellationRequested)
                    return;

                var attempt = 0;

                while (attempt < 50 && !stoppingToken.IsCancellationRequested)
                {
                    attempt++;
                    try
                    {
                        await connection.StartAsync(stoppingToken);
                        _logger.LogInformation("Reconexão manual bem-sucedida!");
                        break;
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, $"Tentativa [{attempt}]: Erro ao tentar reconectar ao SignalR manualmente.");
                        _logger.LogError(ex, $"Tentando novamente em 40 segundos...");
                        for (int i = 0; i < 30; i++)
                        {
                            if (stoppingToken.IsCancellationRequested)
                                return;

                            var delay = TimeSpan.FromSeconds(Math.Min(5 * attempt, 60));
                            await Task.Delay(delay, CancellationToken.None);
                        }
                    }
                }

                if (attempt >= 50)
                    _logger.LogCritical("Falha ao reconectar ao SignalR após 50 tentativas. Verifique a conectividade.");
            };
        }
    }
}
