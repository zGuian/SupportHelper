using SupportHelper.WinServices.Core.Interfaces;

namespace SupportHelper.WinServices.Core.Workers
{
    public class MachineInfoWorker : BackgroundService
    {
        private readonly ILogger<MachineInfoWorker> _logger;
        private readonly IConfiguration _configuration;
        private readonly IMachineService _machineService;

        public MachineInfoWorker(ILogger<MachineInfoWorker> logger, IMachineService machineService, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
            _machineService = machineService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("MachineWorker iniciado.");
            await _machineService.GetInformationFromMachineAsync(_configuration, stoppingToken);
        }
    }
}
