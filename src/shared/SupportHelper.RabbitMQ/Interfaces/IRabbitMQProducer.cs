using RabbitMQ.Client;

namespace SupportHelper.RabbitMQ.Interfaces
{
    public interface IRabbitMQProducer
    {
        IModel Channel { get; }
        Task PublishAsync(string exchange, string routingKey, IBasicProperties properties, 
            string message, CancellationToken cancellationToken = default);
        Task PublishAsync(string exchange, IBasicProperties properties, string message, CancellationToken cancellationToken = default);
    }
}
