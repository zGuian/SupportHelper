using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using SupportHelper.Communication.Requests;
using SupportHelper.Communication.Responses;
using SupportHelper.RabbitMQ.Interfaces;
using SupportHelper.WinServices.Core.Interfaces;
using SupportHelper.WinServices.Core.Models;
using System.Text;
using System.Text.Json;

namespace SupportHelper.WinServices.Core.Services
{
    public sealed class MachineService : IMachineService
    {
        private readonly ILogger<MachineService> _logger;
        private readonly IRabbitMQConnection _connection;

        public MachineService(ILogger<MachineService> logger, IRabbitMQConnection connection)
        {
            _logger = logger;
            _connection = connection;
        }

        public async Task GetInformationFromMachineAsync(IConfiguration configuration, CancellationToken cancellationToken)
        {
            var channel = await _connection.DeclareQueueAndExchange(Environment.MachineName, cancellationToken);
            var consumer = new AsyncEventingBasicConsumer(channel);
            consumer.ReceivedAsync += async (model, ea) =>
            {
                try
                {
                    var body = ea.Body.ToArray();
                    var correlationId = ea.BasicProperties.CorrelationId;
                    var messageJson = Encoding.UTF8.GetString(body);
                    var request = JsonSerializer.Deserialize<MachineInformationRequest>(messageJson) ?? throw new Exception();
                    await channel.BasicAckAsync(ea.DeliveryTag, false);
                    _logger.LogInformation("Recebida mensagem com CorrelationId: {CorrelationId}", ea.BasicProperties.CorrelationId);

                    var machine = new MachineModel();
                    machine.GetAllInformationFromMachine();
                    var response = new ResponseBase<MachineModel>(true, machine);

                    var responseBody = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(response));

                    var properties = new BasicProperties
                    {
                        CorrelationId = correlationId,
                        ContentType = "application/json",
                        ContentEncoding = "UTF8"
                    };

                    await channel.BasicPublishAsync("", ea.BasicProperties.ReplyTo, false, properties, responseBody);
                    _logger.LogInformation("Resposta enviada para fila '{ReplyTo}'", ea.BasicProperties.ReplyTo);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Erro ao processar a mensagem");
                }
                await Task.Yield();
            };

            await channel.BasicConsumeAsync(configuration["RabbitMQ:ConfigExchange:QueueNameDefault"]!, autoAck: false, consumer, cancellationToken);
        }
    }
}