using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using SupportHelper.Communication.Requests;
using SupportHelper.Communication.Responses;
using SupportHelper.Domain.Entities;
using SupportHelper.Domain.Interfaces.MQServices;
using SupportHelper.RabbitMQ.Interfaces;
using System.Collections.Concurrent;
using System.Text;
using System.Text.Json;
using System.Threading;

namespace SupportHelper.Infrastructure.MQServices
{
    public class MachineMQServices : IMachineMQServices
    {
        private readonly ILogger<MachineMQServices> _logger;
        private readonly IRabbitMQConnection _connection;
        private readonly IConfiguration _configuration;
        private readonly ConcurrentDictionary<string, TaskCompletionSource<Machine>> _pendingRequests = new();


        public MachineMQServices(ILogger<MachineMQServices> logger, IRabbitMQConnection connection, IConfiguration configuration)
        {
            _logger = logger;
            _connection = connection;
            _configuration = configuration;
            _ = StartReplyConsumerAsync();
        }

        public async Task<Guid> PublishByRouteKey(MachineInformationRequest request)
        {
            using var channel = await _connection.CreateQueueAndExchange(request.RabbitMQRequest.Hostname);
            var json = JsonSerializer.Serialize(request);
            var body = Encoding.UTF8.GetBytes(json);
            var correlationId = Guid.NewGuid();
            var properties = new BasicProperties
            {
                ReplyTo = request.RabbitMQRequest.ReplyToQueueName,
                DeliveryMode = DeliveryModes.Persistent,
                ContentType = "application/json",
                ContentEncoding = "UTF8",
                CorrelationId = correlationId.ToString()
            };

            var routingKey = $"information.machine.{request.RabbitMQRequest.Hostname.ToLower()}";
            await channel.BasicPublishAsync(request.RabbitMQRequest.Exchange, routingKey, false, properties, body);
            _logger.LogInformation("Mensagem publicada na exchange");
            return correlationId;
        }

        public async Task<Machine> ConsumeReplyTo()
        {
            var channel = await _connection.Connection.CreateChannelAsync();
            var tcs = new TaskCompletionSource<Machine>(TaskCreationOptions.RunContinuationsAsynchronously);

            var consumer = new AsyncEventingBasicConsumer(channel);
            consumer.ReceivedAsync += async (model, ea) =>
            {
                try
                {
                    var body = ea.Body.ToArray();
                    var messageJson = Encoding.UTF8.GetString(body);
                    var response = JsonSerializer.Deserialize<ResponseBase<Machine>>(messageJson)
                                   ?? throw new Exception("Resposta inválida");
                    await channel.BasicAckAsync(ea.DeliveryTag, false);
                    _logger.LogInformation("Recebida mensagem com CorrelationId: {CorrelationId}", ea.BasicProperties.CorrelationId);
                    tcs.SetResult(response.Value);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Erro ao processar resposta");
                    tcs.SetException(ex);
                }
                await Task.Yield();
            };

            await channel.BasicConsumeAsync(
                queue: _configuration["RabbitMQ:ConfigExchange:QueueNameDefault"]!,
                autoAck: false,
                consumer: consumer
            );

            return await tcs.Task;
        }

        private async Task StartReplyConsumerAsync()
        {
            var channel = await _connection.Connection.CreateChannelAsync();
            var consumer = new AsyncEventingBasicConsumer(channel);
            consumer.ReceivedAsync += async (model, ea) =>
            {
                var correlationId = ea.BasicProperties.CorrelationId;
                try
                {
                    var body = ea.Body.ToArray();
                    var json = Encoding.UTF8.GetString(body);
                    var response = JsonSerializer.Deserialize<ResponseBase<Machine>>(json);
                    if (response != null && _pendingRequests.TryRemove(correlationId, out var tcs))
                    {
                        tcs.SetResult(response.Value);
                        _logger.LogInformation("Resposta recebida com CorrelationId: {CorrelationId}", correlationId);
                    }
                    await channel.BasicAckAsync(ea.DeliveryTag, false);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Erro ao processar resposta da fila reply");
                }
                await Task.Yield();
            };

            await channel.BasicConsumeAsync(
                queue: _configuration["RabbitMQ:ConfigExchange:QueueNameDefault"]!,
                autoAck: false,
                consumer: consumer
            );
        }
    }
}
