using RabbitMQ.Client;

namespace SupportHelper.RabbitMQ.Interfaces
{
    public interface IRabbitMQConnection
    {
        IConnection Connection { get; }

        Task<IChannel> DeclareQueueAndExchange(string hostname, CancellationToken  cancellationToken = default);
        Task<IChannel> DeclareQueueAndExchange(CancellationToken cancellationToken = default);
        Task<(IChannel, string queueReply)> DeclareQueueForReplyTo(CancellationToken cancellationToken = default);
    }
}
