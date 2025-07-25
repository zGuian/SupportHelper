using Microsoft.Extensions.Logging;
using SupportHelper.Communication.Requests;
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

        public async Task<string> MakeAvailableLogSgpClient(RequestLogsSgpClientJson request)
        {
            try
            {
                var filePath = string.Empty;
                _logger.LogInformation("Iniciando processo de copiar arquivos");
                await Task.Run(() => { _getLoggerUseCase.Execute(request.ProductionLine, out filePath); });
                if (filePath != "ERROR")
                {
                    await _fileTranferHandler.SendArchiveZipAsync(filePath, request.DestinyArchive);
                    return "OK - Arquivo enviado com sucesso";
                }
                return "NOK - NÃO POSSIVEL COLETAR AS LOGS. NÃO ENCONTRADO ARQUIVOS DE LOG";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "HOUVE UM PROBLEMA AO ENVIAR ARQUIVOS DE LOG \nERROR: {ex} \n\n", ex.StackTrace);
                return "NOK - HOUVE UM PROBLEMA AO ENVIAR ARQUIVOS DE LOG";
            }
        }

        public async Task<string> MakeAvailableLogSgpClient(string productionLine, string destinyArchived)
        {
            try
            {
                var filePath = string.Empty;
                _logger.LogInformation("Iniciando processo de copiar arquivos");
                await Task.Run(() => { _getLoggerUseCase.Execute(productionLine, out filePath); });
                await _fileTranferHandler.SendArchiveZipAsync(filePath, destinyArchived);
                return "OK - Arquivo enviado com sucesso";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Houve um problema ao enviar o arquivo");
                return "NOK - Houve um problema ao enviar o arquivo";
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