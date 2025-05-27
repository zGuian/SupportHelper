using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;
using SupportHelper.RabbitMQ.Interfaces;

namespace SupportHelper.RabbitMQ.Implementation
{
    public class RabbitMQConnection : IRabbitMQConnection, IAsyncDisposable
    {
        private readonly IConnection _connection;
        private bool _disposed;

        private RabbitMQConnection(IConnection connection)
        {
            _connection = connection;
        }

        public IConnection Connection
        {
            get
            {
                return _disposed ? throw new ObjectDisposedException(GetType().FullName) : _connection;
            }
        }

        public static async Task<RabbitMQConnection> CreateConnectionToRabbitMQ(IConfiguration configuration)
        {
            var section = configuration.GetSection("RabbitMQ:Config");
            if (section == null || !section.Exists())
            {
                throw new ArgumentNullException("RabbitMQ:Config section not found in configuration");
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
                return new RabbitMQConnection(connection);
            }
            catch (BrokerUnreachableException ex)
            {
                throw new Exception("Failed to create RabbitMQ connection", ex.InnerException);
            }
        }

        public async ValueTask DisposeAsync()
        {
            if (_disposed) return;

            if (_connection.IsOpen)
            {
                await _connection.CloseAsync();
            }
            await _connection.DisposeAsync();

            _disposed = true;
        }
    }
}
