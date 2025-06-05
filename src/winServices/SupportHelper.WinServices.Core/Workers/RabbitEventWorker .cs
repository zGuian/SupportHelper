
using Microsoft.Extensions.Configuration;
using SupportHelper.WinServices.Core.Events;

namespace SupportHelper.WinServices.Core.Workers
{
    internal class RabbitEventWorker : BackgroundService
    {
        private readonly RabbitMQEvent _rabbitEvent;
        private readonly IConfiguration _configuration;

        public RabbitEventWorker(RabbitMQEvent rabbitEvent, IConfiguration configuration)
        {
            _rabbitEvent = rabbitEvent;
            _configuration = configuration;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await _rabbitEvent.ListenRabbitQueueDefault(_configuration, stoppingToken);
        }
    }
}
