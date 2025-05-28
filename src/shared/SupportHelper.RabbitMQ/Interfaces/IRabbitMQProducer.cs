using RabbitMQ.Client;

namespace SupportHelper.RabbitMQ.Interfaces
{
    public interface IRabbitMQProducer
    {
        Task<string> PublishAsync(string exchange, string routingKey, string message, string? correlationId = null, bool persistent = true, IDictionary<string, object?>? headers = null);
    }
}
