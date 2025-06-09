using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using SupportHelper.Communication.Responses;
using SupportHelper.Domain.Entities;
using SupportHelper.Domain.Interfaces.Repositories;
using SupportHelper.Infrastructure.Contracts;
using System.Text;
using System.Text.Json;

namespace SupportHelper.Infrastructure.MQServices.MQResponses
{
    public class MachineConsumer : IMachineConsumer
    {
        private readonly ILogger<MachineConsumer> _logger;
        private readonly IServiceProvider _serviceProvider;
        private readonly IRabbitMQConnection _connection;

        public MachineConsumer(IServiceProvider serviceProvider, IRabbitMQConnection connection,
            ILogger<MachineConsumer> logger)
        {
            _serviceProvider = serviceProvider;
            _connection = connection;
            _logger = logger;
        }

        public async Task ListenRabbitQueueDefault(IConfiguration configuration, CancellationToken cancellationToken)
        {
            var channel = await _connection.DeclareExchangeAndQueueDefaultAsync(cancellationToken);
            var consumer = new AsyncEventingBasicConsumer(channel);
            consumer.ReceivedAsync += async (_, ea) =>
            {
                try
                {
                    var body = ea.Body.ToArray();
                    var messageJson = Encoding.UTF8.GetString(body);
                    var machine = JsonSerializer.Deserialize<ResponseBase<Machine>>(messageJson) ?? throw new Exception();
                    using var scope = _serviceProvider.CreateScope();
                    var repository = scope.ServiceProvider.GetRequiredService<IMachineRepository>();
                    await repository.InsertMachineByProcedure(machine.Value);
                    await channel.BasicAckAsync(ea.DeliveryTag, false);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Erro ao enviar mensagem");
                }
                await Task.Yield();
            };

            await channel.BasicConsumeAsync(queue: configuration["RabbitMQ:ConfigExchange:ReplyToDefault"]!,
                                            autoAck: false,
                                            consumer: consumer,
                                            cancellationToken: cancellationToken);
        }
    }
}
