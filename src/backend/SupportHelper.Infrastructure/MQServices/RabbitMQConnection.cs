using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using SupportHelper.Infrastructure.Contracts;

namespace SupportHelper.Infrastructure.MQServices
{
    public class RabbitMQConnection : IRabbitMQConnection
    {
        private readonly IConfiguration _configuration;
        private readonly Lazy<Task<IConnection>> _lazyConnection;
        private IChannel? Channel { get; set; }
        Dictionary<string, string> IRabbitMQConnection.ConfigurationValue => ExchangeConfiguration();

        public RabbitMQConnection(IConfiguration configuration)
        {
            _configuration = configuration;
            _lazyConnection = new Lazy<Task<IConnection>>(CreateConnectionAsync);
        }

        public async Task<IChannel> DeclareExchangeAndQueueDefaultAsync(CancellationToken cancellationToken = default)
        {
            var connection = await _lazyConnection.Value;
            var dict = ExchangeConfiguration();

            if (Channel == null)
            {
                var newChannel = await connection.CreateChannelAsync(cancellationToken: cancellationToken);
                Channel = newChannel;
            }

            await Channel.ExchangeDeclareAsync(dict["exchange"],
                                               type: ExchangeType.Direct,
                                               durable: true,
                                               autoDelete: false,
                                               cancellationToken: cancellationToken);

            await Channel.QueueDeclareAsync(queue: dict["queueDefault"],
                                            durable: false,
                                            exclusive: false,
                                            autoDelete: false,
                                            cancellationToken: cancellationToken);
            return Channel;
        }

        public async Task<IChannel> DeclareExchangeAndQueueReplyTo(CancellationToken cancellationToken = default)
        {
            var connection = await _lazyConnection.Value;
            var dict = ExchangeConfiguration();

            if (Channel == null)
            {
                var newChannel = await connection.CreateChannelAsync(cancellationToken: cancellationToken);
                Channel = newChannel;
            }

            await Channel.ExchangeDeclareAsync(dict["exchange"], ExchangeType.Direct, durable: true,
                autoDelete: false, cancellationToken: cancellationToken);

            await Channel.QueueDeclareAsync(queue: dict["queueReplyto"],
                                            durable: false,
                                            exclusive: false,
                                            autoDelete: false,
                                            cancellationToken: cancellationToken);

            return Channel;
        }

        private async Task<IConnection> CreateConnectionAsync()
        {
            var section = _configuration.GetSection($"RabbitMQ:Configuration");
            if (section == null || !section.Exists())
            {
                throw new ArgumentNullException("RabbitMQ:Config section not found in configuration");
            }
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

        public Dictionary<string, string> ExchangeConfiguration()
        {
            var section = _configuration.GetSection("RabbitMQ:ConfigExchange");
            if (section == null || !section.Exists())
            {
                throw new ArgumentNullException("RabbitMQ:Config section not found in configuration");
            }
            var exchange = section["ExchangeDefault"]!;
            var queueDefault = section["QueueNameDefault"]!;
            var queueReplyto = section["ReplyToDefault"]!;

            return new Dictionary<string, string>
            {
                { nameof(exchange), exchange },
                { nameof(queueDefault), queueDefault },
                { nameof(queueReplyto), queueReplyto },
            };
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
