using SupportHelper.Application.Interfaces;
using SupportHelper.Domain.Interfaces.Repositories.Memory;
using SupportHelper.Domain.Interfaces.SignalRContext;

namespace SupportHelper.Application.UseCases.MachineUC
{
    public class RequestStatusMachineUseCase : IRequestStatusMachineUseCase
    {
        private readonly IMachineSignalRServices _signalR;
        private readonly IMachineMemoryRepository _memoryRepository;

        public RequestStatusMachineUseCase(IMachineSignalRServices signalR, IMachineMemoryRepository memoryRepository)
        {
            _signalR = signalR;
            _memoryRepository = memoryRepository;
        }

        public async Task ExecuteAsync(string hostname)
        {
            var connId = _memoryRepository.GetConnectionId(hostname);
            await _signalR.RequestStatusAsync(connId);
        }
    }
}
