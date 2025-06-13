using SupportHelper.WinServices.Core.Interfaces.Services;
using SupportHelper.WinServices.Core.Interfaces.UseCases;
using SupportHelper.WinServices.Core.Models;

namespace SupportHelper.WinServices.Core.Services
{
    public sealed class MachineService : IMachineService
    {
        private readonly ILogger<MachineService> _logger;
        private readonly IGetLoggerSgpClientUseCase _getLoggerUseCase;
        private readonly IUpdateSgpClientUseCase _updateSgpClientUseCase;

        public MachineService(ILogger<MachineService> logger, IGetLoggerSgpClientUseCase getLoggerUseCase, IUpdateSgpClientUseCase updateSgpClientUseCase)
        {
            _logger = logger;
            _getLoggerUseCase = getLoggerUseCase;
            _updateSgpClientUseCase = updateSgpClientUseCase;
        }

        public MachineModel GetInformationMachine(CancellationToken cancellationToken = default)
        {
            var machine = new MachineModel();
            machine.GetAllInformationFromMachine();
            _logger.LogInformation("Encontrado informações da máquina: {machine}", machine.Hostname);
            return machine;
        }

        public async Task<bool> MakeAvailableLogSgpClient(string productionLine)
        {
            try
            {
                _logger.LogInformation("Iniciando processo de copiar arquivos");
                await Task.Run(() =>
                {
                    _getLoggerUseCase.Execute(productionLine);
                });
                return true;
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError("Houve um problema: {message}", ex.Message);
                return false;
            }
        }

        public async Task<bool> UpdateSgpClient(string productionLine)
        {
            try
            {
                _logger.LogInformation("Iniciando processo para atualizar o SGP via Start");
                await Task.Run(() =>
                {
                    _updateSgpClientUseCase.Execute(productionLine);
                });
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError("Ocorreu um erro ao atualizar o SGP: {message}", ex.Message);
                return false;
            }
        }
    }
}