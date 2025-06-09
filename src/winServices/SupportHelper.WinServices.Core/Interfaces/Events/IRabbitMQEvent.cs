namespace SupportHelper.WinServices.Core.Interfaces.Events
{
    public interface IRabbitMQEvent
    {
        Task ListenRabbitQueueDefault(IConfiguration configuration, CancellationToken cancellationToken);
    }
}
