using Microsoft.Extensions.Configuration;

namespace SupportHelper.Infrastructure.Contracts
{
    public interface IMachineConsumer
    {
        Task ListenRabbitQueueDefault(IConfiguration configuration, CancellationToken cancellationToken);
    }
}
