using RabbitMQ.Client;

namespace SupportHelper.RabbitMQ.Interfaces
{
    public interface IRabbitMQProducer
    {
        Task PublishAsync<T>(string exchange, string routingKey, T message, bool persistent = true, IDictionary<string, object?>? headers = null);
    }
}
