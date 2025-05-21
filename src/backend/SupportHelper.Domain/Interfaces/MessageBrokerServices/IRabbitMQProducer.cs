namespace SupportHelper.Domain.Interfaces.MessageBrokerServices
{
    public interface IRabbitMQProducer
    {
        Task Publisher(string messageJson, IDictionary<string, object> headers = null);
    }
}
