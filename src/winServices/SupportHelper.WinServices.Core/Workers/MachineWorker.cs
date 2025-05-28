using Microsoft.Extensions.Configuration;
using SupportHelper.WinServices.Core.Interfaces;

namespace SupportHelper.WinServices.Core.Workers
{
    public class MachineWorker : BackgroundService
    {
        private readonly ILogger<MachineWorker> _logger;
        private readonly IMachineService _machineService;
        private readonly IConfiguration _configuration;

        public MachineWorker(ILogger<MachineWorker> logger, IMachineService machineService, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
            _machineService = machineService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("MachineWorker iniciado.");
            var section = _configuration.GetSection("RabbitMQ:Config");
            if (section == null || !section.Exists())
            {
                throw new ArgumentNullException("RabbitMQ:Config section not found in configuration");
            }
            var exchange = section["ExchangeDefault"]!;
            var replyTo = section["ReplyTo"]!;
            var queueNameDefault = section["QueueNameDefault"]!;
            await _machineService.GetInformationFromMachineAsync(exchange, queueNameDefault, cancellationToken: stoppingToken);
            _logger.LogInformation("MachineWorker finalizado.");
        }
    }
}
