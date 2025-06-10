using SupportHelper.Infrastructure.Contracts;

namespace SupportHelper.WebApi.Workers
{
    public class RabbitMQListenWorker : BackgroundService
    {
        private readonly ILogger<RabbitMQListenWorker> _logger;
        private readonly IServiceScopeFactory _factory;
        private readonly IConfiguration _configuration;

        public RabbitMQListenWorker(ILogger<RabbitMQListenWorker> logger, 
            IConfiguration configuration, IServiceScopeFactory factory)
        {
            _configuration = configuration;
            _logger = logger;
            _factory = factory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using var scope = _factory.CreateScope();
            var consumer = scope.ServiceProvider.GetRequiredService<IMachineConsumer>();
            await consumer.ListenRabbitQueueDefault(_configuration, stoppingToken);
        }
    }
}
