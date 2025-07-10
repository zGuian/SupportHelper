using SupportHelper.Application.Interfaces;
using SupportHelper.Domain.Interfaces.Repositories.Memory;
using SupportHelper.Domain.Interfaces.SignalRContext;

namespace SupportHelper.Application.UseCases.MachineUC
{
    public class RequestStatusMachineUseCase : IRequestStatusMachineUseCase
    {
        private readonly IMachineSignalRServices _signalR;
        private readonly IConnectionMemoryRepository _connectionMemoryRepository;

        public RequestStatusMachineUseCase(IMachineSignalRServices signalR, IConnectionMemoryRepository connectionMemoryRepository)
        {
            _signalR = signalR;
            _connectionMemoryRepository = connectionMemoryRepository;
        }

        public async Task ExecuteAsync(string hostname)
        {
            string connId = _connectionMemoryRepository.GetConnectionId(hostname) ?? throw new Exception("NÃO ENCONTRADO ID DA CONEXÃO");
            await _signalR.RequestStatusAsync(connId);
        }
    }
}
