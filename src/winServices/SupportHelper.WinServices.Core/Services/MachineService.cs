using SupportHelper.WinServices.Core.Interfaces.Services;
using SupportHelper.WinServices.Core.Interfaces.UseCases;
using SupportHelper.WinServices.Core.Models;
using SupportHelper.WinServices.Core.Models.Enums;

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

        public void MakeAvailableLogSgpClient(SGPClientLine productionLine)
        {
            try
            {
                _logger.LogInformation("Iniciando processo de copiar arquivos");
                _getLoggerUseCase.Execute(productionLine);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError("Houve um problema: {message}", ex.Message);
                //IMPLEMENTAR LOGICA PARA ENVIAR ERRO PARA UMA FILA TEMPORARIA CRIADA
            }            
        } 
    }
}