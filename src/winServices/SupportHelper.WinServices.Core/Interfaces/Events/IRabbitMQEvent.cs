using RabbitMQ.Client;

namespace SupportHelper.WinServices.Core.Interfaces.Events
{
    public interface IRabbitMQEvent
    {
        Task ListenRabbitQueueDefault(IChannel channel, IConfiguration configuration, CancellationToken cancellationToken);
    }
}
