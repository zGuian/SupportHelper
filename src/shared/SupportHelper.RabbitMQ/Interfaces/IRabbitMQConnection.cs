using RabbitMQ.Client;

namespace SupportHelper.RabbitMQ.Interfaces
{
    public interface IRabbitMQConnection
    {
        IConnection Connection { get; }

        Task<IChannel> CreateQueueAndExchange(string hostname, CancellationToken  cancellationToken = default);
    }
}
