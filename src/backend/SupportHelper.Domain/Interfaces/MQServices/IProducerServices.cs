namespace SupportHelper.Domain.Interfaces.MQServices
{
    public interface IProducerServices
    {
        Task PublishMessage(string routingKey, string message, CancellationToken cancellationToken = default);
        Task PublishMessage(Dictionary<string, object> headers, string message, CancellationToken cancellationToken = default);
    }
}
