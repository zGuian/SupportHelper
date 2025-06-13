using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;

namespace SupportHelper.WinServices.Application.Interfaces.Events
{
    public interface IRabbitMQEvent
    {
        Task ListenRabbitQueueDefault(IChannel channel, IConfiguration configuration, CancellationToken cancellationToken);
    }
}
