using SupportHelper.Domain.Interfaces.MessageBrokerServices;
using SupportHelper.RabbitMQ.Interfaces;

namespace SupportHelper.Infrastructure.MessageBrokeServices
{
    public class ProducerServices : IProducerServices
    {
        private readonly IRabbitMQProducer _producer;

        public ProducerServices(IRabbitMQProducer producer)
        {
            _producer = producer;
        }

        public async Task PublishMessage(string message)
        {
            var exchange = "";
            var routingKey = "";
            var properties = _producer.channel.CreateBasicProperties();
            properties.ReplyTo = "ReplyTo";
            await Task.Run(() => _producer.Publisher(exchange, routingKey, properties, message));
        }
    }
}
