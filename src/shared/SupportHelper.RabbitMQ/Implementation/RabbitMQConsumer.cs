using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using SupportHelper.RabbitMQ.Interfaces;

namespace SupportHelper.RabbitMQ.Implementation
{
    public class RabbitMQConsumer : IRabbitMQConsumer
    {
        private readonly ILogger<RabbitMQConsumer> _logger;
        private readonly IRabbitMQConnection _connection;

        public RabbitMQConsumer(IRabbitMQConnection connection, ILogger<RabbitMQConsumer> logger)
        {
            _logger = logger;
            _connection = connection;
        }

        public async Task ConsumeAllMessagesAsync(string queueName,
            Func<ReadOnlyMemory<byte>, IReadOnlyBasicProperties, Task> onMessageReceived,
        CancellationToken cancellationToken)
        {
            using var channel = await _connection.Connection.CreateChannelAsync();
            var consumer = new AsyncEventingBasicConsumer(channel);
            consumer.ReceivedAsync += async (sender, args) =>
            {
                try
                {
                    await onMessageReceived(args.Body, args.BasicProperties);
                    await channel.BasicAckAsync(args.DeliveryTag, multiple: true);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Erro ao processar mensagem da fila '{QueueName}'", queueName);
                    await channel.BasicNackAsync(args.DeliveryTag, multiple: false, requeue: true);
                    throw;
                }
            };

            var consumerTag = await channel.BasicConsumeAsync(queue: queueName,
                                                              autoAck: false,
                                                              consumer: consumer,
                                                              cancellationToken);
            try
            {
                await Task.Delay(Timeout.Infinite, cancellationToken);
            }
            catch (TaskCanceledException)
            {
                _logger.LogInformation("Cancelando consumo da fila '{QueueName}'", queueName);
                await channel.BasicCancelAsync(consumerTag);
            }
        }

        public async Task ListenAsync(string exchange, string routingKey, string queueName, 
            Func<ReadOnlyMemory<byte>, IReadOnlyBasicProperties, Task> onMessageReceived, 
            CancellationToken cancellationToken = default)
        {
            await using var channel = await _connection.Connection.CreateChannelAsync(cancellationToken: cancellationToken);

            await channel.ExchangeDeclareAsync(
                exchange: exchange,
                type: ExchangeType.Direct,
                durable: true,
                autoDelete: false,
                cancellationToken: cancellationToken);

            await channel.QueueDeclareAsync(
                queue: queueName,
                durable: true,
                exclusive: false,
                autoDelete: false,
                cancellationToken: cancellationToken);

            await channel.QueueBindAsync(
                queue: queueName,
                exchange: exchange,
                routingKey: routingKey,
                cancellationToken: cancellationToken);

            var consumer = new AsyncEventingBasicConsumer(channel);

            consumer.ReceivedAsync += async (sender, args) =>
            {
                try
                {
                    await onMessageReceived(args.Body, args.BasicProperties);
                    await channel.BasicAckAsync(args.DeliveryTag, multiple: false, cancellationToken);
                }
                catch (Exception ex)
                {
                    await channel.BasicNackAsync(args.DeliveryTag, multiple: false, requeue: true, cancellationToken);
                    Console.WriteLine($"Erro ao processar mensagem: {ex.Message}");
                }
            };

            var consumerTag = await channel.BasicConsumeAsync(
                queue: queueName,
                autoAck: false,
                consumer: consumer,
                cancellationToken: cancellationToken);


            try
            {
                await Task.Delay(Timeout.Infinite, cancellationToken);
            }
            catch (TaskCanceledException)
            {
                Console.WriteLine("Cancelando consumo da fila.");
                await channel.BasicCancelAsync(consumerTag, cancellationToken: cancellationToken);
            }
        }

        public async Task<(ReadOnlyMemory<byte> Body, IReadOnlyBasicProperties Props)> WaitForMessageAsync(
            string queueName, CancellationToken cancellationToken)
        {
            var tcs = new TaskCompletionSource<(ReadOnlyMemory<byte>, IReadOnlyBasicProperties)>();

            await using var channel = await _connection.Connection.CreateChannelAsync(cancellationToken: cancellationToken);

            var consumer = new AsyncEventingBasicConsumer(channel);

            consumer.ReceivedAsync += async (sender, args) =>
            {
                tcs.SetResult((args.Body, args.BasicProperties));
                await channel.BasicAckAsync(args.DeliveryTag, false);
            };

            var consumerTag = await channel.BasicConsumeAsync(queueName, false, consumer, cancellationToken);

            await using (cancellationToken.Register(() => tcs.TrySetCanceled()))
            {
                var result = await tcs.Task;
                await channel.BasicCancelAsync(consumerTag, cancellationToken: cancellationToken);
                return result;
            }
        }
    }
}
