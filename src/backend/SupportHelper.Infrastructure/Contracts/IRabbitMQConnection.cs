using RabbitMQ.Client;

namespace SupportHelper.Infrastructure.Contracts
{
    public interface IRabbitMQConnection
    {
        Dictionary<string, string> ConfigurationValue { get; }
        Task<IChannel> DeclareExchangeAndQueueDefaultAsync(CancellationToken cancellationToken = default);
        Task<IChannel> DeclareExchangeAndQueueReplyTo(CancellationToken cancellationToken = default);
    }
}
