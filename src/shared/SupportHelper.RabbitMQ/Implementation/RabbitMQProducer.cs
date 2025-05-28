using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using SupportHelper.RabbitMQ.Interfaces;
using System.Text;

namespace SupportHelper.RabbitMQ.Implementation
{
    public class RabbitMQProducer : IRabbitMQProducer
    {
        public readonly ILogger<RabbitMQProducer> _logger;
        private readonly IRabbitMQConnection _connection;

        public RabbitMQProducer(IRabbitMQConnection connection, ILogger<RabbitMQProducer> logger)
        {
            _logger = logger;
            _connection = connection;
        }

        public async Task<string> PublishAsync(string exchange, string routingKey, string message,
            bool persistent = true, IDictionary<string, object?>? headers = null)
        {
            ArgumentNullException.ThrowIfNull(message);
            try
            {
                using var channel = await _connection.Connection.CreateChannelAsync();
                var correlationId = Guid.NewGuid().ToString();
                var body = Encoding.UTF8.GetBytes(message);

                await channel.ExchangeDeclareAsync(exchange, type: ExchangeType.Direct,
                    durable: true, autoDelete: false);

                var properties = new BasicProperties
                {
                    ReplyTo = "Reply-To-Information",
                    DeliveryMode = persistent ? DeliveryModes.Persistent : DeliveryModes.Transient,
                    ContentType = "application/json",
                    ContentEncoding = "UTF8",
                    CorrelationId =
                };

                if (headers != null)
                {
                    properties.Headers = headers;
                }

                await channel.BasicPublishAsync(exchange, routingKey, mandatory: true, properties, body);
                _logger.LogInformation("Mensagem publicada na exchange '{Exchange}' com routingKey '{RoutingKey}'",
                exchange, routingKey);
                return correlationId;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao publicar mensagem no RabbitMQ.");
                throw;
            }
        }
    }
}
