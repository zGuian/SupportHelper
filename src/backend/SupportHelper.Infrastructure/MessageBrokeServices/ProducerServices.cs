using Microsoft.Extensions.Configuration;
using SupportHelper.Domain.Interfaces.MessageBrokerServices;
using SupportHelper.RabbitMQ.Interfaces;

namespace SupportHelper.Infrastructure.MessageBrokeServices
{
    public class ProducerServices : IProducerServices
    {
        private readonly IRabbitMQProducer _producer;
        private readonly IConfiguration _configuration;

        public ProducerServices(IConfiguration configuration, IRabbitMQProducer producer)
        {
            _configuration = configuration;
            _producer = producer;
        }

        public async Task PublishMessage(string routingKey, string message, CancellationToken cancellationToken = default)
        {
            var exchange = GetSectionValue("Exchange");
            var properties = _producer.Channel.CreateBasicProperties();
            properties.ReplyTo = GetSectionValue("ReplyTo");
            properties.ContentType = "Application/Json";
            await _producer.PublishAsync(exchange, routingKey, properties, message, cancellationToken);
        }

        public async Task PublishMessage(Dictionary<string, object> headers, string message, CancellationToken cancellationToken = default)
        {
            var exchange = GetSectionValue("Exchange");
            var properties = _producer.Channel.CreateBasicProperties();
            properties.ReplyTo = GetSectionValue("ReplyTo");
            properties.ContentType = "Application/Json";
            properties.Headers = headers;
            properties.ContentEncoding = "UTF8";
            properties.MessageId = Guid.NewGuid().ToString();
            await _producer.PublishAsync(exchange, properties, message, cancellationToken);
        }

        private string GetSectionValue(string key)
        {
            var section = _configuration.GetSection("RabbitMQ:ExchangeConfig");
            return section[key] ?? throw new ArgumentNullException(section[key]);
        }
    }
}
