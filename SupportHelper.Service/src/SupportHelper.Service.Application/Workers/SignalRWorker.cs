using Microsoft.AspNetCore.SignalR.Client;
using SupportHelper.Service.Application.HostedServices;
using SupportHelper.Service.Domain.Events;
using SupportHelper.Service.Domain.Interface.EventHandler;

namespace SupportHelper.Service.Application.Workers
{
    public class SignalRWorker(ILogger<SignalRWorker> logger
        , IEnumerable<ISignalREventHandler> signalREventHandlers
        , IEnumerable<IMachineEventHandler> machineHandlers
        , IConfiguration configuration
        , MachineEvents machineEvents) : BackgroundService
    {
        private HubConnection? _connection;
        private readonly ILogger<SignalRWorker> _logger = logger;
        private readonly MachineEvents _machineEvents = machineEvents;
        private EventRegistration? _eventRegistration;
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

            _eventRegistration = new EventRegistration(signalREventHandlers, machineHandlers, machineEvents, _connection);
            await _eventRegistration.RegisterEvents(stoppingToken);

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
            catch(TaskCanceledException)
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
            _machineEvents.MachineShutdown();
            await Task.Delay(TimeSpan.FromSeconds(10));

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
