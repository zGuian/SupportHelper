using RabbitMQ.Client;
using SupportHelper.Domain.Interfaces.MessageBrokerServices;
using System.Text;

namespace SupportHelper.Infrastructure.MessageBrokeServices
{
    public class RabbitMQProducer : IRabbitMQProducer
    {
        public async Task Publisher(string messageJson, IDictionary<string, object> headers = null)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };

            using var connection = await factory.CreateConnectionAsync();
            using var channel = await connection.CreateChannelAsync();
            await channel.ExchangeDeclareAsync(exchange: "headers_exchange",
                                               type: ExchangeType.Headers,
                                               durable: true);

            var messageBody = Encoding.UTF8.GetBytes(messageJson);
            await channel.BasicPublishAsync(exchange: "headers_exchange",
                                            routingKey: string.Empty,
                                            mandatory: true,
                                            body: messageBody);
        }
    }
}
