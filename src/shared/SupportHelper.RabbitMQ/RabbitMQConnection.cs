using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using SupportHelper.RabbitMQ.Interfaces;

namespace SupportHelper.RabbitMQ
{
    public class RabbitMQConnection : IRabbitMQConnection, IDisposable
    {
        private readonly IConnection _connection;

        public RabbitMQConnection(IConfiguration configuration)
        {
            var section = configuration.GetSection("RabbitMQ:Config");
            var hostname = section["Hostname"];
            var username = section["Username"];
            var password = section["Password"];
            var virtualHost = section["VirtualHost"];
            var port = section["Port"];
            var factory = new ConnectionFactory
            {
                HostName = hostname ?? throw new ArgumentNullException(null, nameof(hostname)),
                UserName = username ?? throw new ArgumentNullException(null, nameof(username)),
                Password = password ?? throw new ArgumentNullException(password, nameof(password)),
                VirtualHost = virtualHost ?? throw new ArgumentNullException(virtualHost, nameof(virtualHost)),
                Port = int.Parse(port ?? throw new ArgumentNullException(null, nameof(port))),
            };
            _connection = factory.CreateConnectionAsync().GetAwaiter().GetResult();
        }

        public IConnection Connection => _connection;

        public void Dispose()
        {
            if (_connection.IsOpen)
            {
                _connection.CloseAsync().GetAwaiter().GetResult();
            }
            _connection.Dispose();
        }
    }
}
