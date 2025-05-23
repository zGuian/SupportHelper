using RabbitMQ.Client;
using SupportHelper.Domain.Interfaces.MessageBrokerServices;
using SupportHelper.RabbitMQ.Interfaces;
using System.Text;

namespace SupportHelper.Infrastructure.MessageBrokeServices
{
    public class RabbitMQProducer : IRabbitMQProducer
    {
        private readonly IRabbitMQConnection _connection;

        public RabbitMQProducer(IRabbitMQConnection connection)
        {
            _connection = connection;
        }

        public Task Publisher(string messageJson, IDictionary<string, string> headers = null)
        {
            throw new NotImplementedException();
        }

        public async Task Publisher(string exchange, string routingKey, string message)
        {
            using var channel = await _connection.Connection.CreateChannelAsync();
            var body = Encoding.UTF8.GetBytes(message);

            await channel.BasicPublishAsync(exchange: exchange,
                                            routingKey: routingKey,
                                            mandatory: false,
                                            body: body);
        }
    }
}
