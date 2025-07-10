using Microsoft.AspNetCore.SignalR.Client;
using SupportHelper.WinServices.Application.Interfaces.Events;

namespace SupportHelper.WinServices.Core.Workers
{
    public class SignalRWorker : BackgroundService
    {
        private HubConnection? _connection;
        private readonly ILogger<SignalRWorker> _logger;
        private readonly IEnumerable<ISignalREventHandler> _signalrHandlers;
        private readonly IWatchForShutdownHandler _watchForShutdownHandler;

        public SignalRWorker(ILogger<SignalRWorker> logger, IEnumerable<ISignalREventHandler> signalREventHandlers,
            IWatchForShutdownHandler watchForShutdownHandler)
        {
            _logger = logger;
            _signalrHandlers = signalREventHandlers;
            _watchForShutdownHandler = watchForShutdownHandler;
        }

        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            string url = "http://localhost:5001/SupportHelperConnectionSignalR?hostname=";

            _connection = new HubConnectionBuilder()
                .WithUrl($"{url}{Environment.MachineName}")
                .WithAutomaticReconnect()
                .Build();

            foreach (ISignalREventHandler handler in _signalrHandlers)
            {
                handler.Register(_connection, stoppingToken);
            }

            _watchForShutdownHandler.Run(_connection, stoppingToken);

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
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _watchForShutdownHandler.Stop(cancellationToken);
            return base.StopAsync(cancellationToken);
        }
    }
}
