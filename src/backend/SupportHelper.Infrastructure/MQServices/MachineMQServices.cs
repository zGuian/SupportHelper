using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SupportHelper.Domain.Entities;
using SupportHelper.Domain.Interfaces.MQServices;
using SupportHelper.RabbitMQ.Interfaces;
using System.Text;
using System.Text.Json;

namespace SupportHelper.Infrastructure.MQServices
{
    public class MachineMQServices : IMachineMQServices
    {
        private readonly ILogger<MachineMQServices> _logger;
        private readonly IRabbitMQConsumer _rabbitConsumer;
        private readonly IRabbitMQProducer _rabbitProducer;
        private readonly IConfiguration _configuration;

        public MachineMQServices(IRabbitMQConsumer rabbitConsumer, IRabbitMQProducer rabbitProducer,
            ILogger<MachineMQServices> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
            _rabbitConsumer = rabbitConsumer;
            _rabbitProducer = rabbitProducer;
        }

        public async Task<Machine?> GetInformationOnlyMachineAsync(string exchange, string routingKey, string message,
            Dictionary<string, object?>? headers = null, CancellationToken cancellationToken = default)
        {
            var section = _configuration.GetSection("RabbitMQ:Config");
            if (section == null || !section.Exists())
            {
                throw new ArgumentNullException("RabbitMQ:Config section not found in configuration");
            }
            var replyTo = section["ReplyToDefault"]!;
            var correlationId = await _rabbitProducer.PublishAsync(exchange, routingKey, message, headers: headers);
            var timeoutCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            timeoutCts.CancelAfter(TimeSpan.FromMinutes(1));
            while (true)
            {
                try
                {
                    var (body, props) = await _rabbitConsumer.WaitForMessageAsync(replyTo, timeoutCts.Token);
                    if (props.CorrelationId == correlationId)
                    {
                        _logger.LogInformation("Encontrado resposta via RabbitMQ");
                        message = Encoding.UTF8.GetString(body.Span);
                        var machine = JsonSerializer.Deserialize<Machine>(message);
                        return machine;
                    }
                }
                catch (TaskCanceledException)
                {
                    _logger.LogError("Timeout ao aguardar a resposta.");
                    return null;
                }
            }
        }
    }
}
