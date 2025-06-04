using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using SupportHelper.RabbitMQ.Interfaces;
using System.Collections.Concurrent;
using System.Text;
using System.Text.Json;

namespace SupportHelper.RabbitMQ.Implementation
{
    public class RabbitMQRequestReply<TRequest, TResponse> : IRabbitMQRequestReply<TRequest, TResponse>
    {
        private readonly IRabbitMQConnection _connection;
        private readonly ILogger<RabbitMQRequestReply<TRequest, TResponse>> _logger;
        private readonly ConcurrentDictionary<string, TaskCompletionSource<TResponse>> _pendingReplies = new();
        private IChannel? _replyChannel;
        private string? _replyQueue;

        public RabbitMQRequestReply(ILogger<RabbitMQRequestReply<TRequest, TResponse>> logger,
            IRabbitMQConnection connection)
        {
            _logger = logger;
            _connection = connection;
        }

        public async Task StartConsumerAsync()
        {
            (_replyChannel, string queue) = await _connection.DeclareQueueForReplyTo();
            _replyQueue = queue;

            var consumer = new AsyncEventingBasicConsumer(_replyChannel);
            consumer.ReceivedAsync += async (_, ea) =>
            {
                var correlationId = ea.BasicProperties?.CorrelationId;
                if (string.IsNullOrWhiteSpace(correlationId)) return;

                try
                {
                    var json = Encoding.UTF8.GetString(ea.Body.ToArray());
                    var response = JsonSerializer.Deserialize<TResponse>(json);

                    if (response != null && _pendingReplies.TryRemove(correlationId, out var tcs))
                    {
                        tcs.TrySetResult(response);
                    }
                    await _replyChannel.BasicAckAsync(ea.DeliveryTag, false);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Erro ao processar resposta com CorrelationId: {CorrelationId}", correlationId);
                    await _replyChannel.BasicNackAsync(ea.DeliveryTag, false, false);
                }
            };

            await _replyChannel.BasicConsumeAsync(_replyQueue, false, consumer);
            _logger.LogInformation("Consumidor de reply iniciado para {ReplyQueue}", _replyQueue);
        }

        public async Task<TResponse> SendAsync(TRequest request, string routingKey, string exchange)
        {
            if (_replyChannel == null || _replyQueue == null)
                throw new InvalidOperationException("O consumidor de reply não foi iniciado.");

            var correlationId = Guid.NewGuid().ToString();
            var tcs = new TaskCompletionSource<TResponse>(TaskCreationOptions.RunContinuationsAsynchronously);
            _pendingReplies[correlationId] = tcs;

            var channel = await _connection.DeclareQueueAndExchange();

            var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(request));
            var props = new BasicProperties
            {
                CorrelationId = correlationId,
                ReplyTo = _replyQueue,
                ContentType = "application/json",
                ContentEncoding = "UTF8",
                DeliveryMode = DeliveryModes.Persistent
            };

            await channel.BasicPublishAsync(exchange, routingKey, false, props, body);
            _logger.LogInformation("Mensagem enviada para {RoutingKey} com CorrelationId {CorrelationId}", routingKey, correlationId);

            var timeout = Task.Delay(TimeSpan.FromMinutes(1));
            var completed = await Task.WhenAny(tcs.Task, timeout);

            if (completed == timeout)
            {
                _pendingReplies.TryRemove(correlationId, out _);
                throw new TimeoutException("Timeout na espera da resposta.");
            }

            return await tcs.Task;
        }
    }
}
