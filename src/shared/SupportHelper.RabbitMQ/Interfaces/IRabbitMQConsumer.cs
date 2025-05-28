using RabbitMQ.Client;

namespace SupportHelper.RabbitMQ.Interfaces
{
    public interface IRabbitMQConsumer
    {
        Task ConsumeAllMessagesAsync(string queueName, Func<ReadOnlyMemory<byte>, IReadOnlyBasicProperties, 
            Task> onMessageReceived, CancellationToken cancellationToken);

        Task<(ReadOnlyMemory<byte> Body, IReadOnlyBasicProperties Props)> WaitForMessageAsync(string queueName, 
            CancellationToken cancellationToken);

        Task ListenAsync(string exchange, string routingKey, string queueName, 
            Func<ReadOnlyMemory<byte>, IReadOnlyBasicProperties, Task> onMessageReceived, 
            CancellationToken cancellationToken = default);
    }
}
