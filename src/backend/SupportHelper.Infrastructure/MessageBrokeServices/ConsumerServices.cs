using SupportHelper.Domain.Entities;
using SupportHelper.Domain.Interfaces.MessageBrokerServices;
using SupportHelper.RabbitMQ.Interfaces;
using System.Text.Json;

namespace SupportHelper.Infrastructure.MessageBrokeServices
{
    public sealed class ConsumerServices : IConsumeServices
    {
        private readonly IRabbitMQConsumer _consumer;

        public ConsumerServices(IRabbitMQConsumer consumer)
        {
            _consumer = consumer;
        }

        public async Task<Machine> ConsumeMessageAsync(string queueName, bool autoAck)
        {
            var message = await _consumer.StartConsuming(queueName, autoAck);
            var machine = JsonSerializer.Deserialize<Machine>(message) ?? throw new Exception();
            return machine;
            throw new NotImplementedException();
        }

        public string ConsumeMessage()
        {
            throw new NotImplementedException();
        }
    }
}
