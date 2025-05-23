using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using SupportHelper.RabbitMQ.Interfaces;

namespace SupportHelper.RabbitMQ
{
    public class RabbitMQConnection : IRabbitMQConnection, IDisposable
    {
        private readonly IConfiguration _configuration;
        private readonly IConnection _connection;

        public RabbitMQConnection(IConfiguration configuration)
        {
            _configuration = configuration;
            var factory = new ConnectionFactory
            {

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
