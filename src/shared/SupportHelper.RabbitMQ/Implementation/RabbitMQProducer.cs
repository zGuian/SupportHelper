using RabbitMQ.Client;
using SupportHelper.RabbitMQ.Interfaces;
using System.Text;

namespace SupportHelper.RabbitMQ.Implementation
{
    public class RabbitMQProducer : IRabbitMQProducer
    {
        private readonly IModel _channel;

        public RabbitMQProducer(IRabbitMQConnection connection)
        {
            _channel = connection.CreateChannel();
        }

        public void Publisher(string exchange, string routingKey, IBasicProperties properties, string message)
        {
            var body = Encoding.UTF8.GetBytes(message);
            _channel.BasicPublish(exchange, routingKey, properties, body);
        }
        
        public IModel channel => _channel;
    }
}
