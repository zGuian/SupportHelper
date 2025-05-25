using RabbitMQ.Client;
using SupportHelper.RabbitMQ.Exceptions;
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

        public IModel Channel => _channel;

        public async Task PublishAsync(string exchange, string routingKey, IBasicProperties properties, string message, CancellationToken cancellationToken = default)
        {
            try
            {
                var body = Encoding.UTF8.GetBytes(message);
                await Task.Run(() => _channel.BasicPublish(exchange, routingKey, properties, body), cancellationToken);
            }
            catch (Exception ex)
            {
                throw new RabbitMQPublishException(ex.Message, ex.InnerException);
            }
        }

        public async Task PublishAsync(string exchange, IBasicProperties properties, string message, CancellationToken cancellationToken = default)
        {
            try
            {
                var body = Encoding.UTF8.GetBytes(message);
                await Task.Run(() => _channel.BasicPublish(exchange, string.Empty, properties, body), cancellationToken);
            }
            catch (Exception ex)
            {
                throw new RabbitMQPublishException(ex.Message, ex.InnerException);
            }
        }
    }
}
