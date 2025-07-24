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
        private readonly IFileTransferUseCase _fileTranferHandler;

        public MachineService(ILogger<MachineService> logger, IGetLoggerSgpClientUseCase getLoggerUseCase,
            IUpdateSgpClientUseCase updateSgpClientUseCase, IFileTransferUseCase fileTranferHandler)
        {
            _logger = logger;
            _getLoggerUseCase = getLoggerUseCase;
            _updateSgpClientUseCase = updateSgpClientUseCase;
            _fileTranferHandler = fileTranferHandler;
        }

        public async Task<bool> MakeAvailableLogSgpClient(string productionLine, string requestId)
        {
            try
            {
                _logger.LogInformation("Iniciando processo de copiar arquivos");
                var pathArchiveZip = _getLoggerUseCase.Execute(productionLine);
                await _fileTranferHandler.SendArchiveZipAsync(productionLine, pathArchiveZip, requestId);

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Houve um problema ao enviar o arquivo");
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