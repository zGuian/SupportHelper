using RabbitMQ.Client;

namespace SupportHelper.RabbitMQ.Interfaces
{
    public interface IRabbitMQConnection
    {
        IModel CreateChannel();
    }
}
