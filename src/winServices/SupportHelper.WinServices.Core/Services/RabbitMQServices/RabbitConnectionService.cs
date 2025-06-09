using RabbitMQ.Client;
using SupportHelper.WinServices.Core.Interfaces.RabbitMQService;

namespace SupportHelper.WinServices.Core.Services.RabbitMQServices
{
    public class RabbitConnectionService : IRabbitConnectionService, IDisposable
    {
        private readonly IConfiguration _configuration;
        private readonly Lazy<Task<IConnection>> _lazyConnection;

        public RabbitConnectionService(IConfiguration configuration)
        {
            _configuration = configuration;
            _lazyConnection = new Lazy<Task<IConnection>>(CreateConnectionAsync);
        }

        public async Task<IChannel> DeclareQueueAndExchange(string hostname, CancellationToken cancellationToken = default)
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

            var routingKey = $"worker.machine.{hostname.ToLower()}";

            await channel.QueueBindAsync(queueDefault, exchange, routingKey, cancellationToken: cancellationToken);

            return channel;
        }

        public async Task<(IChannel, string)> DeclareQueueForReplyTo(CancellationToken cancellationToken = default)
        {
            var connection = await _lazyConnection.Value;
            var section = GetSection("ConfigExchange");
            var exchange = section["ExchangeDefault"]!;
            var queueReplyTo = section["ReplyToDefault"]!;
            var channel = await connection.CreateChannelAsync(cancellationToken: cancellationToken);

            await channel.ExchangeDeclareAsync(exchange: exchange,
                                               type: ExchangeType.Direct,
                                               durable: true,
                                               autoDelete: false,
                                               cancellationToken: cancellationToken);

            await channel.QueueDeclareAsync(queue: queueReplyTo,
                                            durable: false,
                                            exclusive: false,
                                            autoDelete: false,
                                            cancellationToken: cancellationToken);

            await channel.QueueBindAsync(queue: queueReplyTo,
                                         exchange: exchange,
                                         routingKey: "",
                                         cancellationToken: cancellationToken);

            return (channel, queueReplyTo);
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
