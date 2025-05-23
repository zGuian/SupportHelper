using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using SupportHelper.RabbitMQ.Interfaces;
using System.Text;
using System.Text.Json;

namespace SupportHelper.RabbitMQ.Implementation
{
    public class RabbitMQConsumer : IRabbitMQConsumer
    {
        private readonly IModel _channel;

        public RabbitMQConsumer(IRabbitMQConnection connection)
        {
            _channel = connection.CreateChannel();
        }

        public void StartConsuming<T>(string queueName, bool autoAck)
        {
            var consumer = new AsyncEventingBasicConsumer(_channel);
            consumer.Received += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var json = JsonSerializer.Deserialize<T>(Encoding.UTF8.GetString(body));
                await Task.CompletedTask;

                _channel.BasicAck(ea.DeliveryTag, multiple: false);
            };

            _channel.BasicConsume(queueName, autoAck, consumer);
        }
    }
}
