using Microsoft.AspNetCore.SignalR.Client;
using SupportHelper.Service.CrossCutting.Bootstrapper;
using SupportHelper.Service.Domain.Interface.Workers;

namespace SupportHelper.Service.Application.Workers
{
    public class SignalRWorker(HubConnection connection
        , ILogger<SignalRWorker> logger
        , EventRegistrationBootstrapper events) : BackgroundService, ISignalRWorker
    {
        public event EventHandler? OnMachineShutdown;
        private readonly EventRegistrationBootstrapper _events = events;
        private readonly HubConnection _connection = connection;
        private readonly ILogger<SignalRWorker> _logger = logger;

        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _events.RegisterEvents(stoppingToken);
            try
            {
                await _connection.StartAsync(stoppingToken);
                _logger.LogInformation("Conectado ao SignalR. ConnectionId: {id}", _connection.ConnectionId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao conectar com o SignalR");
            }

            try
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    await Task.Delay(TimeSpan.FromSeconds(15), stoppingToken);
                }
            }
            catch (TaskCanceledException)
            {
                _logger.LogInformation("Serviço cancelado com segurança.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro inesperado no SignalRWorker.");
            }
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            OnMachineShutdown?.Invoke(this, EventArgs.Empty);
            await Task.Delay(TimeSpan.FromSeconds(15), cancellationToken);

            if (_connection is not null)
            {
                try
                {
                    await _connection.StopAsync(cancellationToken);
                    await _connection.DisposeAsync();
                    _logger.LogInformation("Conexão SignalR encerrada com sucesso.");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Erro ao encerrar conexão SignalR.");
                }
            }

            await base.StopAsync(cancellationToken);
        }
    }
}
