using RabbitMQ.Client;

namespace SupportHelper.RabbitMQ.Interfaces
{
    public interface IRabbitMQProducer
    {
        IModel channel { get; }
        void Publisher(string exchange, string routingKey, IBasicProperties properties, string message);
    }
}
