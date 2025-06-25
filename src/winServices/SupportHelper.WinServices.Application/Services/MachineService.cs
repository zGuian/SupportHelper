using Microsoft.Extensions.Logging;
using SupportHelper.WinServices.Application.Interfaces.Services;
using SupportHelper.WinServices.Application.Interfaces.UseCases;

namespace SupportHelper.WinServices.Application.Services
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

        public async Task<string> UpdateSgpClient(string productionLine)
        {
            try
            {
                _logger.LogInformation("Iniciando processo para atualizar o SGP via Start");
                await Task.Run(() =>
                {
                    return _updateSgpClientUseCase.Execute(productionLine);
                });
                return "";
            }
            catch (Exception ex)
            {
                _logger.LogError("Ocorreu um erro ao atualizar o SGP: {message}", ex.Message);
                return "";
            }
        }
    }
}