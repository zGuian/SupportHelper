using SupportHelper.WinServices.Application.Interfaces.Events;
using SupportHelper.WinServices.Application.Interfaces.RabbitMQService;

namespace SupportHelper.WinServices.Core.Workers
{
    internal class RabbitEventWorker : BackgroundService
    {
        private readonly IRabbitMQEvent _rabbitEvent;
        private readonly IConfiguration _configuration;
        private readonly IRabbitConnectionService _connection;

        public RabbitEventWorker(IRabbitMQEvent rabbitEvent, IConfiguration configuration,
            IRabbitConnectionService connection)
        {
            _rabbitEvent = rabbitEvent;
            _configuration = configuration;
            _connection = connection;
        }

        public override async Task StartAsync(CancellationToken cancellationToken)
        {
            await base.StartAsync(cancellationToken);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var channel = await _connection.DeclareQueueAndExchange(Environment.MachineName.ToLower(), stoppingToken);
            await _rabbitEvent.ListenRabbitQueueDefault(channel, _configuration, stoppingToken);
        }
    }
}
