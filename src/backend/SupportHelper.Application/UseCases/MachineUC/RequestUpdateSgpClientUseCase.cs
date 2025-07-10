using SupportHelper.Application.Interfaces;
using SupportHelper.Communication.Requests;
using SupportHelper.Communication.Responses;
using SupportHelper.Domain.Interfaces.Repositories.Memory;
using SupportHelper.Domain.Interfaces.SignalRContext;

namespace SupportHelper.Application.UseCases.MachineUC
{
    public sealed class RequestUpdateSgpClientUseCase : IRequestUpdateSgpClientUseCase
    {
        private readonly IMachineSignalRServices _signalRService;
        private readonly IConnectionMemoryRepository _connectionMemoryRepository;

        public RequestUpdateSgpClientUseCase(IMachineSignalRServices signalRService, IConnectionMemoryRepository connectionMemoryRepository)
        {
            _signalRService = signalRService;
            _connectionMemoryRepository = connectionMemoryRepository;
        }

        public async Task<ResponseUpdateSgpClientJson> ExecuteAsync(RequestUpdateSgpClientJson requestJson)
        {
            string? connectionId = _connectionMemoryRepository.GetConnectionId(requestJson.Hostname) 
                ?? throw new Exception("Não foi encontrado o id da conexão do equipamento");
            ResponseUpdateSgpClientJson response = await _signalRService.UpdateSgpClientAsync(connectionId, requestJson.ProductionLine);
            return response;
        }
    }
}
