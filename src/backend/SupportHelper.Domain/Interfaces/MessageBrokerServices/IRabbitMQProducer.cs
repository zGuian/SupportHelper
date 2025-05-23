namespace SupportHelper.Domain.Interfaces.MessageBrokerServices
{
    public interface IRabbitMQProducer
    {
        Task Publisher(string messageJson, IDictionary<string, string> headers = null);
        Task Publisher(string exchenge, string routeKey, string message);
    }
}
