namespace SupportHelper.WebApi.Workers
{
    [Obsolete("Não utilizar este worker. Removido RabbitMQ")]
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

        [Obsolete("Não utilizar este construtor, é apenas para compatibilidade com o Worker do Hangfire")]
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            //using var scope = _factory.CreateScope();
            //var consumer = scope.ServiceProvider.GetRequiredService<IMachineConsumer>();
            //await consumer.ListenRabbitQueueDefault(_configuration, stoppingToken);
        }
    }
}
