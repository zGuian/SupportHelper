using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using SupportHelper.Communication.Requests;
using SupportHelper.Domain.Interfaces.MQServices;
using SupportHelper.Infrastructure.Contracts;
using System.Text;
using System.Text.Json;

namespace SupportHelper.Infrastructure.MQServices
{
    public sealed class MachineMQService : IMachineMQServices
    {
        private readonly ILogger<MachineMQService> _logger;
        private readonly IRabbitMQConnection _connection;

        public MachineMQService(IRabbitMQConnection connection, ILogger<MachineMQService> logger)
        {
            _connection = connection;
            _logger = logger;
        }

        public async Task PublishMessageAsync(RequestBase<RequestMachine> request, RabbitMQRequest rabbitMQRequest)
        {
            try
            {
                var channel = await _connection.DeclareExchangeAndQueueDefaultAsync();
                var correlationId = Guid.NewGuid().ToString();

                var jsonString = JsonSerializer.Serialize(request);
                var body = Encoding.UTF8.GetBytes(jsonString);
                var properties = new BasicProperties
                {
                    CorrelationId = correlationId,
                    ContentType = "application/json",
                    ContentEncoding = "UTF8",
                    Timestamp = new AmqpTimestamp(),
                    ReplyTo = "queue.response.machines"
                };

                var routingKey = $"worker.machine.{rabbitMQRequest.Hostname}";

                await channel.BasicPublishAsync(exchange: _connection.ConfigurationValue["exchange"],
                                                routingKey: routingKey,
                                                mandatory: true,
                                                basicProperties: properties,
                                                body: body);

                _logger.LogInformation("publicado mensagem");
            }
            catch (Exception ex)
            {
                _logger.LogError("Erro ao publicar mensagem. Error {message}", ex.Message);
                throw;
            }
        }

        public async Task PublishMessageAsync(MachineInformationRequest request)
        {
            try
            {
                var channel = await _connection.DeclareExchangeAndQueueDefaultAsync();
                var correlationId = Guid.NewGuid().ToString();

                var jsonString = JsonSerializer.Serialize(request);
                var body = Encoding.UTF8.GetBytes(jsonString);
                var properties = new BasicProperties
                {
                    CorrelationId = correlationId,
                    ContentType = "application/json",
                    ContentEncoding = "UTF8",
                    Timestamp = new AmqpTimestamp(),
                    ReplyTo = "queue.response.machines"
                };

                var routingKey = $"worker.machine.{request.RabbitMQRequest.Hostname}";

                await channel.BasicPublishAsync(exchange: _connection.ConfigurationValue["exchange"],
                                                routingKey: routingKey,
                                                mandatory: true,
                                                basicProperties: properties,
                                                body: body);

                _logger.LogInformation("publicado mensagem");
            }
            catch (Exception ex)
            {
                _logger.LogError("Erro ao publicar mensagem. Error {message}", ex.Message);
                throw;
            }
        }
    }
}
