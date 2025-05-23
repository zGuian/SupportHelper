namespace SupportHelper.Domain.Interfaces.MessageBrokerServices
{
    public interface IProducerServices
    {
        Task PublishMessage(string message);
    }
}
