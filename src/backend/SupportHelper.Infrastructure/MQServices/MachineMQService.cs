using RabbitMQ.Client;
using SupportHelper.Communication.Requests;
using SupportHelper.Domain.Interfaces.MQServices;
using SupportHelper.Infrastructure.Contracts;
using System.Text;
using System.Text.Json;

namespace SupportHelper.Infrastructure.MQServices
{
    public class MachineMQService : IMachineMQServices
    {

        private readonly IRabbitMQConnection _connection;

        public MachineMQService(IRabbitMQConnection connection)
        {
            _connection = connection;
        }

        public async Task PublishGetInformationAsync(MachineInformationRequest request)
        {
            var channel = await _connection.DeclareExchangeAndQueueDefaultAsync();
            var correlationId = Guid.NewGuid().ToString();
            var jsonString = JsonSerializer.Serialize(request);
            var body = Encoding.UTF8.GetBytes(jsonString);
            var properties = new BasicProperties
            {
                CorrelationId = correlationId,
                ContentType = "application/json",
                ContentEncoding = "UTF8"
            };

            var routingKey = $"machine.information.{request.RabbitMQRequest.Hostname.ToLower()}";

            await channel.BasicPublishAsync(exchange: request.RabbitMQRequest.Exchange,
                                            routingKey: routingKey,
                                            mandatory: true,
                                            basicProperties: properties,
                                            body: body);
        }
    }
}
