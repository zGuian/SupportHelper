using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using SupportHelper.RabbitMQ.Interfaces;

namespace SupportHelper.RabbitMQ.Implementation
{
    public class RabbitMQConnection : IRabbitMQConnection, IHostedService
    {
        private readonly IConfiguration _configuration;
        public IConnection Connection { get; private set; } = default!;

        public RabbitMQConnection(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<IChannel> CreateQueueAndExchange(string hostname, CancellationToken cancellationToken = default)
        {
            var section = _configuration.GetSection("RabbitMQ:ConfigExchange");
            if (section == null || !section.Exists())
            {
                throw new ArgumentNullException("RabbitMQ:Config section not found in configuration");
            }
            var exchange = section["ExchangeDefault"]!;
            var queueDefault = section["QueueNameDefault"]!;
            var channel = await Connection.CreateChannelAsync(cancellationToken: cancellationToken);

            await channel.ExchangeDeclareAsync(exchange, ExchangeType.Direct, durable: true,
                autoDelete: false, cancellationToken: cancellationToken);

            await channel.QueueDeclareAsync(queue: queueDefault, autoDelete: false, 
                cancellationToken: cancellationToken);

            var routingKey = $"worker.machine.{hostname.ToLower()}";

            await channel.QueueBindAsync(queueDefault, exchange, routingKey, cancellationToken: cancellationToken);

            return channel;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            var section = _configuration.GetSection("RabbitMQ:Configuration");
            if (!section.Exists())
                throw new InvalidOperationException("RabbitMQ:Configuration not found.");

            var factory = new ConnectionFactory
            {
                Uri = new Uri("amqp://localhost:5672"),
                HostName = section["Hostname"]!,
                UserName = section["Username"]!,
                Password = section["Password"]!,
                VirtualHost = section["VirtualHost"]!,
                Port = int.TryParse(section["Port"]!, out var parsedPort) ? parsedPort : AmqpTcpEndpoint.UseDefaultPort,
                AutomaticRecoveryEnabled = true,
                NetworkRecoveryInterval = TimeSpan.FromSeconds(30)
            };
            Connection = await factory.CreateConnectionAsync(cancellationToken);
            Console.WriteLine("[RabbitMQ] - Connection established.");
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine("[RabbitMQ] - Closing connection...");
            await Connection.CloseAsync(cancellationToken);
            Connection?.Dispose();
        }
    }
}
