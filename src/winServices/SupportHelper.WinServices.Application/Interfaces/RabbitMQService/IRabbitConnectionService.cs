using RabbitMQ.Client;

namespace SupportHelper.WinServices.Application.Interfaces.RabbitMQService
{
    public interface IRabbitConnectionService
    {
        Task<IChannel> DeclareQueueAndExchange(string hostname, CancellationToken cancellationToken = default);
        Task<(IChannel, string)> DeclareQueueForReplyTo(CancellationToken cancellationToken = default);
    }
}
