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

        public async Task<IChannel> DeclareQueueAndExchange(string hostname, CancellationToken cancellationToken = default)
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

        public async Task<(IChannel, string)> DeclareQueueForReplyTo(CancellationToken cancellationToken = default)
        {
            var section = _configuration.GetSection("RabbitMQ:ConfigExchange");
            if (section == null || !section.Exists())
            {
                throw new ArgumentNullException("RabbitMQ:Config section not found in configuration");
            }
            var exchange = section["ExchangeDefault"]!;
            var queueReplyTo = section["ReplyToDefault"]!;
            var channel = await Connection.CreateChannelAsync(cancellationToken: cancellationToken);

            await channel.ExchangeDeclareAsync(exchange: exchange,
                                               type: ExchangeType.Direct,
                                               durable: true,
                                               autoDelete: false,
                                               cancellationToken: cancellationToken);

            await channel.QueueDeclareAsync(queue: queueReplyTo,
                                            autoDelete: false,
                                            cancellationToken: cancellationToken);

            await channel.QueueBindAsync(queue: queueReplyTo,
                                         exchange: exchange,
                                         routingKey: "",
                                         cancellationToken: cancellationToken);

            return (channel, queueReplyTo);
        }

        public async Task<IChannel> DeclareQueueAndExchange(CancellationToken cancellationToken = default)
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

            var routingKey = $"worker.machine.console";

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
                Uri = new Uri(section["Url"]!),
                UserName = section["Username"]!,
                Password = section["Password"]!,
                VirtualHost = section["VirtualHost"]!,
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
