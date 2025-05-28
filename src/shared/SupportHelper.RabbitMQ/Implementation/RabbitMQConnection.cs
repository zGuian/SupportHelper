using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;
using SupportHelper.RabbitMQ.Interfaces;

namespace SupportHelper.RabbitMQ.Implementation
{
    public class RabbitMQConnection : IRabbitMQConnection, IHostedService
    {
        public IConnection Connection { get; private set; } = default!;
        private readonly IConnection _connection;
        private readonly IConfiguration _configuration;

        private RabbitMQConnection(IConnection connection, IConfiguration configuration)
        {
            _connection = connection;
            _configuration = configuration;
        }

        public static async Task<RabbitMQConnection> CreateConnectionToRabbitMQ(IConfiguration configuration)
        {
            var section = configuration.GetSection("RabbitMQ:Configuration");
            if (!section.Exists())
            {
                throw new InvalidOperationException("RabbitMQ:Configuration section not found in configuration.");
            }
            var hostname = section["Hostname"];
            var username = section["Username"];
            var password = section["Password"];
            var virtualHost = section["VirtualHost"];
            var port = section["Port"];
            if (string.IsNullOrEmpty(hostname))
                throw new ArgumentNullException(nameof(hostname));
            if (string.IsNullOrEmpty(username))
                throw new ArgumentNullException(nameof(username));
            if (string.IsNullOrEmpty(password))
                throw new ArgumentNullException(nameof(password));
            var factory = new ConnectionFactory
            {
                HostName = hostname,
                UserName = username,
                Password = password,
                VirtualHost = virtualHost ?? "/",
                Port = port != null ? int.Parse(port) : AmqpTcpEndpoint.UseDefaultPort,
                AutomaticRecoveryEnabled = true,
                NetworkRecoveryInterval = TimeSpan.FromSeconds(10)
            };
            try
            {
                var connection = await factory.CreateConnectionAsync();
                return new RabbitMQConnection(connection, configuration);
            }
            catch (BrokerUnreachableException ex)
            {
                throw new Exception("Failed to create RabbitMQ connection", ex.InnerException);
            }
        }

        public async Task<IChannel> CreateQueueInExchange(string hostname, CancellationToken cancellationToken = default)
        {
            var section = _configuration.GetSection("RabbitMQ:ConfigExchange");
            if (section == null || !section.Exists())
            {
                throw new ArgumentNullException("RabbitMQ:Config section not found in configuration");
            }
            var exchange = section["ExchangeDefault"]!;
            var queueDefault = section["QueueNameDefault"]!;
            var channel = await _connection.CreateChannelAsync();
            await channel.ExchangeDeclareAsync(exchange, ExchangeType.Direct, durable: true,
                autoDelete: false);
            await channel.QueueDeclareAsync(queue: queueDefault, autoDelete: false);
            var routingKey = $"worker.machine.{hostname.ToLower()}";
            await channel.QueueBindAsync(section["QueueNameDefault"]!, section["ExchangeDefault"]!, routingKey);

            return channel;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            var section = _configuration.GetSection("RabbitMQ:Configuration");
            if (!section.Exists())
                throw new InvalidOperationException("RabbitMQ:Configuration not found.");

            var factory = new ConnectionFactory
            {
                HostName = section["Hostname"]!,
                UserName = section["Username"]!,
                Password = section["Password"]!,
                VirtualHost = section["VirtualHost"]!,
                Port = int.TryParse(section["Port"]!, out var parsedPort) ? parsedPort : AmqpTcpEndpoint.UseDefaultPort,
                AutomaticRecoveryEnabled = true,
                NetworkRecoveryInterval = TimeSpan.FromSeconds(30)
            };

            Connection = await factory.CreateConnectionAsync(cancellationToken);

            Console.WriteLine("[RabbitMQ] ✅ Connection established.");
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine("[RabbitMQ] 🛑 Closing connection...");
            await Connection.CloseAsync(cancellationToken);
            Connection?.Dispose();
        }
    }
}
