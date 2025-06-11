using SupportHelper.WinServices.Core.Interfaces;
using SupportHelper.WinServices.Core.Interfaces.UseCases;
using SupportHelper.WinServices.Core.Models;

namespace SupportHelper.WinServices.Core.Services
{
    public sealed class MachineService : IMachineService
    {
        private readonly ILogger<MachineService> _logger;
        private readonly IGetLoggerSgpClientUseCase _getLoggerUseCase;

        public MachineService(ILogger<MachineService> logger, IGetLoggerSgpClientUseCase getLoggerUseCase)
        {
            _logger = logger;
            _getLoggerUseCase = getLoggerUseCase;
        }

        public MachineModel GetInformationMachine(CancellationToken cancellationToken = default)
        {
            var machine = new MachineModel();
            machine.GetAllInformationFromMachine();
            _logger.LogInformation("Encontrado informações da máquina: {machine}", machine.Hostname);
            return machine;
        }

        public async Task MakeAvailableLogSgpClient()
        {
            throw new NotImplementedException();
            //await _getLoggerUseCase.ExecuteAsync();
        } 
    }
}