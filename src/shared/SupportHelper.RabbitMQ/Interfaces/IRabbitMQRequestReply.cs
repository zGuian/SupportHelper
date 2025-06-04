using RabbitMQ.Client;

namespace SupportHelper.RabbitMQ.Interfaces
{
    public interface IRabbitMQRequestReply<TRequest,TResponse>
    {
        Task<TResponse> SendAsync(TRequest request, string routingKey, string exchange);
        Task StartConsumerAsync();
    }
}
