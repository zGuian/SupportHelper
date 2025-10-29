using Microsoft.AspNetCore.SignalR.Client;
using SupportHelper.Service.Domain.Interface.EventHandler;

namespace SupportHelper.Service.Application.Workers
{
    public class SignalRWorker(ILogger<SignalRWorker> logger
            , IEnumerable<ISignalREventHandler> signalREventHandlers
            , IConfiguration configuration) : BackgroundService
    {
        private HubConnection? _connection;
        private readonly ILogger<SignalRWorker> _logger = logger;
        private readonly IEnumerable<ISignalREventHandler> _signalrHandlers = signalREventHandlers;
        private readonly IConfiguration _configuration = configuration;

        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var url = _configuration["SignalrSettings:Url"];
            if (string.IsNullOrEmpty(url))
            {
                throw new ArgumentNullException(nameof(url));
            }

            _connection = new HubConnectionBuilder()
                .WithUrl($"{url}{Environment.MachineName}", opts =>
                {
                    opts.Headers.Add("X-Hostname", Environment.MachineName);
                    opts.Headers.Add("X-CurrentUsername", Environment.UserName);
                    opts.Headers.Add("X-Uptime", TimeSpan.FromMilliseconds(Environment.TickCount64).ToString());
                })
                .WithAutomaticReconnect()
                .WithStatefulReconnect()
                .Build();

            foreach (ISignalREventHandler handler in _signalrHandlers)
            {
                handler.Register(_connection, stoppingToken);
            }

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
                await Task.Delay(TimeSpan.FromSeconds(30), stoppingToken);
            }
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            return base.StopAsync(cancellationToken);
        }
    }
}
