using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using SupportHelper.Infrastructure.Contracts;

namespace SupportHelper.Infrastructure.MQServices
{
    public class RabbitMQConnection : IRabbitMQConnection
    {
        private readonly IConfiguration _configuration;
        private readonly Lazy<Task<IConnection>> _lazyConnection;

        public RabbitMQConnection(IConfiguration configuration)
        {
            _configuration = configuration;
            _lazyConnection = new Lazy<Task<IConnection>>(CreateConnectionAsync);
        }

        public async Task<IChannel> DeclareExchangeAndQueueDefaultAsync(CancellationToken cancellationToken = default)
        {
            var connection = await _lazyConnection.Value;
            var section = GetSection("ConfigExchange");
            var exchange = section["ExchangeDefault"]!;
            var queueDefault = section["QueueNameDefault"]!;
            var channel = await connection.CreateChannelAsync(cancellationToken: cancellationToken);

            await channel.ExchangeDeclareAsync(exchange, ExchangeType.Direct, durable: true,
                autoDelete: false, cancellationToken: cancellationToken);

            await channel.QueueDeclareAsync(queue: queueDefault,
                                            durable: false,
                                            exclusive: false,
                                            autoDelete: false,
                                            cancellationToken: cancellationToken);

            return channel;
        }

        private async Task<IConnection> CreateConnectionAsync()
        {
            var section = GetSection("Configuration");
            var factory = new ConnectionFactory
            {
                Uri = new Uri(section["Url"]!),
                UserName = section["Username"]!,
                Password = section["Password"]!,
                AutomaticRecoveryEnabled = true,
                NetworkRecoveryInterval = TimeSpan.FromSeconds(30)
            };

            var connection = await factory.CreateConnectionAsync();
            Console.WriteLine("[RabbitMQ] - Connection established.");
            return connection;
        }

        private IConfigurationSection GetSection(string s)
        {
            var section = _configuration.GetSection($"RabbitMQ:{s}");
            if (section == null || !section.Exists())
            {
                throw new ArgumentNullException("RabbitMQ:Config section not found in configuration");
            }
            return section;
        }

        public void Dispose()
        {
            if (_lazyConnection.IsValueCreated)
            {
                _lazyConnection.Value.Result?.Dispose();
                Console.WriteLine("[RabbitMQ] - Connection disposed.");
            }
        }
    }
}
