using RabbitMQ.Client;

namespace SupportHelper.WinServices.Core.Interfaces.RabbitMQService
{
    public interface IRabbitConnectionService
    {
        Task<IChannel> DeclareQueueAndExchange(string hostname, CancellationToken cancellationToken = default);
        Task<(IChannel, string)> DeclareQueueForReplyTo(CancellationToken cancellationToken = default);
    }
}
