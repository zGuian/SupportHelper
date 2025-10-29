using Microsoft.Extensions.Logging;
using SupportHelper.Service.Domain.DTOs.Requests;
using SupportHelper.Service.Domain.DTOs.Responses;
using SupportHelper.Service.Domain.Interface.Services;
using SupportHelper.Service.Domain.Interface.UseCases;
using System.Text.Json;

namespace SupportHelper.Service.Domain.Services
{
    public sealed class MachineService(ILogger<MachineService> logger
            , IGetLoggerSgpClientUseCase getLoggerUseCase
            , IUpdateSgpClientUseCase updateSgpClientUseCase
            , IFileTransferUseCase fileTranferHandler
            , IGetStatusMachineUseCase getStatusMachineUseCase) : IMachineServices
    {
        private readonly ILogger<MachineService> _logger = logger;
        private readonly IGetLoggerSgpClientUseCase _getLoggerUseCase = getLoggerUseCase;
        private readonly IUpdateSgpClientUseCase _updateSgpClientUseCase = updateSgpClientUseCase;
        private readonly IFileTransferUseCase _fileTranferHandler = fileTranferHandler;
        private readonly IGetStatusMachineUseCase _getStatusMachineUse = getStatusMachineUseCase;

        public async Task<string> GetStatusToMachine()
        {
            try
            {
                var machine = await Task.Run(() =>
                {
                    var valueMachine = _getStatusMachineUse.Execute();
                    return valueMachine;
                });
                return JsonSerializer.Serialize(machine);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "HOUVE UM PROBLEMA AO COLETAR INFORMAÇÕES DO EQUIPAMENTO \nERROR: {ex} \n\n", ex.Message);
                return "NOK - HOUVE UM PROBLEMA AO COLETAR INFORMAÇÕES DO EQUIPAMENTO";
            }
        }

        public async Task<string> MakeAvailableLogSgpClient(RequestLogsSgpClient request)
        {
            try
            {
                var filePath = string.Empty;
                _logger.LogInformation("Iniciando processo de copiar arquivos");
                await Task.Run(() => { _getLoggerUseCase.Execute(request.ProductionLine, out filePath); });
                if (filePath == "ERROR")
                {
                    return "NOK - NÃO POSSIVEL COLETAR AS LOGS. NÃO ENCONTRADO ARQUIVOS DE LOG";
                }
                var value = await _fileTranferHandler.SendArchiveZipAsync(filePath, request.DestinyArchive);
                if (value == true)
                {
                    var response = new ResponseLogsSgpClient(value, null, request.DestinyArchive);
                    return JsonSerializer.Serialize(response);
                }
                var response2 = new ResponseLogsSgpClient(value,
                    $"Não foi possivel encontrar o arquivo copiado.", request.DestinyArchive);
                return JsonSerializer.Serialize(response2);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "HOUVE UM PROBLEMA AO ENVIAR ARQUIVOS DE LOG \nERROR: {ex} \n\n", ex.StackTrace);
                return "NOK - HOUVE UM PROBLEMA AO ENVIAR ARQUIVOS DE LOG";
            }
        }

        public async Task<string> MakeAvailableLogSgpClient(string productionLine, string requestId)
        {
            try
            {
                var filePath = string.Empty;
                _logger.LogInformation("Iniciando processo de copiar arquivos");
                await _getLoggerUseCase.ExecuteAsync(productionLine, requestId);
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
