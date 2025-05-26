namespace SupportHelper.RabbitMQ.Interfaces
{
    public interface IRabbitMQConsumer
    {
        Task<string> StartConsuming<T>(string queueName, bool autoAck);
        Task<string> StartConsuming(string queueName, bool autoAck);
    }
}
