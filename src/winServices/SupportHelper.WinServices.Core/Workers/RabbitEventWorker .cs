using SupportHelper.WinServices.Core.Interfaces.Events;
using SupportHelper.WinServices.Core.Interfaces.RabbitMQService;

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
            await _connection.DeclareQueueAndExchange(Environment.MachineName.ToLower(), cancellationToken);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await _rabbitEvent.ListenRabbitQueueDefault(_configuration, stoppingToken);
        }
    }
}
