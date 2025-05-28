using SupportHelper.Communication.Requests;
using SupportHelper.Exceptions;
using SupportHelper.Exceptions.ExceptionsBase;
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
        private readonly IRabbitMQProducer _producer;
        private readonly IRabbitMQConsumer _consumer;

        public MachineService(ILogger<MachineService> logger, IRabbitMQProducer producer,
            IRabbitMQConsumer consumer)
        {
            _logger = logger;
            _producer = producer;
            _consumer = consumer;
        }

        public async Task GetInformationFromMachineAsync(string exchange, string routingKey, string queueName,
            CancellationToken cancellationToken = default)
        {
            await _consumer.ListenAsync(exchange, routingKey, queueName,
                onMessageReceived: async (body, props) =>
                {
                    try
                    {
                        var json = Encoding.UTF8.GetString(body.Span);
                        var request = JsonSerializer.Deserialize<MachineInformationRequest>(json) ??
                            throw new GenericErrorException([ResourceMessagesException.GENERIC_ERROR]);
                        var machine = new MachineModel();
                        machine.GetAllInformationFromMachine();
                        var message = JsonSerializer.Serialize(machine);
                        await _producer.PublishAsync(string.Empty, props.ReplyTo!, message, props.CorrelationId);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Erro ao processar a mensagem.");
                        throw;
                    }
                }, cancellationToken);
        }
    }
}