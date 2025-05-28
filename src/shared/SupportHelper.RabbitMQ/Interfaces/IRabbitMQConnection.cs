using RabbitMQ.Client;

namespace SupportHelper.RabbitMQ.Interfaces
{
    public interface IRabbitMQConnection
    {
        IConnection Connection { get; }

        Task<IChannel> CreateQueueInExchange(string hostname, CancellationToken  cancellationToken = default);
    }
}
