using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using SupportHelper.RabbitMQ.Interfaces;

namespace SupportHelper.RabbitMQ.Implementation
{
    public class RabbitMQConnection : IRabbitMQConnection, IDisposable
    {
        private readonly IConnection _connection;
        private bool _disposed = false;

        public bool IsConnected => _connection?.IsOpen ?? false;

        public RabbitMQConnection(IConfiguration configuration)
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
            };
            try
            {
                _connection = factory.CreateConnection();
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to create RabbitMQ connection", ex);
            }
        }

        public IModel CreateChannel()
        {
            if (!IsConnected)
                throw new InvalidOperationException("No RabbitMQ connections are available to perform this action");

            return _connection.CreateModel();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    if (_connection != null)
                    {
                        if (_connection.IsOpen)
                        {
                            _connection.Close();
                        }
                        _connection.Dispose();
                    }
                }
                _disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
