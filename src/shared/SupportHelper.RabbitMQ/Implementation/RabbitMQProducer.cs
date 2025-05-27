using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using SupportHelper.RabbitMQ.Interfaces;
using System.Text;
using System.Text.Json;

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

        public async Task PublishAsync<T>(string exchange, string routingKey, T message,
            bool persistent = true, IDictionary<string, object?>? headers = null)
        {
            if (message == null) throw new ArgumentNullException(nameof(message));
            try
            {
                using var channel = await _connection.Connection.CreateChannelAsync();
                var json = JsonSerializer.Serialize(message);
                var body = Encoding.UTF8.GetBytes(json);

                await channel.ExchangeDeclareAsync(exchange, type: ExchangeType.Direct,
                    durable: true, autoDelete: false);

                var properties = new BasicProperties
                {
                    DeliveryMode = DeliveryModes.Persistent,
                    ContentType = "application/json",
                    ContentEncoding = "UTF8"
                };

                properties = CreateReplyToProperties("Reply-To-Information", persistent);

                if (headers != null)
                {
                    properties.Headers = headers;
                }

                await channel.BasicPublishAsync(exchange, routingKey, mandatory: true, properties, body);
                _logger.LogInformation("Mensagem publicada na exchange '{Exchange}' com routingKey '{RoutingKey}'",
                exchange, routingKey);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao publicar mensagem no RabbitMQ.");
                throw;
            }
        }

        private BasicProperties CreateReplyToProperties(string replyToQueue, bool persistence = true)
        {
            var properties = new BasicProperties
            {
                ReplyTo = replyToQueue,
                ContentType = "application/json",
                CorrelationId = Guid.NewGuid().ToString(),
            };

            if (persistence)
            {
                properties.DeliveryMode = DeliveryModes.Persistent;
            }
            else
            {
                properties.DeliveryMode = DeliveryModes.Transient;
            }
            return properties;
        }
    }
}
