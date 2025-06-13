using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using SupportHelper.Communication.Requests;
using SupportHelper.Communication.Responses;
using SupportHelper.WinServices.Application.Interfaces.Events;
using SupportHelper.WinServices.Application.Interfaces.RabbitMQService;
using SupportHelper.WinServices.Application.Interfaces.Services;
using SupportHelper.WinServices.Core.Models;
using System.Text;
using System.Text.Json;

namespace SupportHelper.WinServices.Core.Events
{
    public class RabbitMQEvent : IRabbitMQEvent
    {
        private readonly IRabbitConnectionService _connection;
        private readonly ILogger<RabbitMQEvent> _logger;
        private readonly IMachineService _machineService;

        public RabbitMQEvent(IRabbitConnectionService connection, ILogger<RabbitMQEvent> logger,
            IMachineService machineService)
        {
            _connection = connection;
            _logger = logger;
            _machineService = machineService;
        }

        public async Task ListenRabbitQueueDefault(IChannel channel, IConfiguration configuration, CancellationToken cancellationToken)
        {
            var consumer = new AsyncEventingBasicConsumer(channel);
            consumer.ReceivedAsync += async (_, ea) =>
            {
                try
                {
                    var body = ea.Body.ToArray();
                    var messageJson = Encoding.UTF8.GetString(body);
                    var request = JsonSerializer.Deserialize<MachineInformationRequest>(messageJson) ?? throw new Exception();
                    await channel.BasicAckAsync(ea.DeliveryTag, false);
                    _logger.LogInformation("Recebida mensagem com CorrelationId: {CorrelationId}", ea.BasicProperties.CorrelationId);
                    await ValidateCommand(channel, ea, request);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Erro ao processar a mensagem");
                }
                await Task.Yield();
            };

            await channel.BasicConsumeAsync(queue: "queue.workers",
                                            autoAck: false,
                                            consumerTag: $"machine-{Environment.MachineName.ToLower()}",
                                            consumer: consumer,
                                            cancellationToken: cancellationToken);
        }

        private async Task ValidateCommand(IChannel channel, BasicDeliverEventArgs ea,
            MachineInformationRequest request)
        {
            dynamic result;
            switch (request.Command)
            {
                case "GET_INFORMATION_MACHINE":
                    MachineModel machine = new();
                    machine.GetAllInformationFromMachine();
                    await PublishReplyTo(channel, ea, machine, true);
                    break;

                case "GET_LOG_SGPCLIENT":
                    result = await _machineService.MakeAvailableLogSgpClient(
                        ea.BasicProperties.Headers!.Keys.FirstOrDefault(x => x.Equals("ProductionLine")) ??
                        throw new Exception("Linha de produção não encontrada"));

                    if (!result)
                        await PublishReplyTo(channel, ea, new { }, false);
                    break;

                case "UPDATE_SGPCLIENT":
                    result = await _machineService.UpdateSgpClient(ea.BasicProperties.Headers!.Keys.FirstOrDefault(x => x.Equals("ProductionLine")) ??
                        throw new Exception("Linha de produção não encontrada"));

                    if (!result)
                        await PublishReplyTo(channel, ea, new { }, false);
                    break;

                default:
                    break;
            }
        }

        private async Task PublishReplyTo<T>(IChannel channel, BasicDeliverEventArgs ea, T model, bool isSuccess)
        {
            var correlationId = ea.BasicProperties.CorrelationId;
            var jsonString = JsonSerializer.Serialize(new ResponseBase<T>(isSuccess, model));
            var responseBody = Encoding.UTF8.GetBytes(jsonString);
            var properties = new BasicProperties
            {
                CorrelationId = correlationId,
                ContentType = "application/json",
                ContentEncoding = "UTF8"
            };

            await channel.BasicPublishAsync("", ea.BasicProperties.ReplyTo!, false, properties, responseBody);
            _logger.LogInformation("Resposta enviada para fila '{ReplyTo}'", ea.BasicProperties.ReplyTo);
        }
    }
}
