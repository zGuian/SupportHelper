using SupportHelper.Application.Interfaces;
using SupportHelper.Domain.Interfaces.SignalRContext;

namespace SupportHelper.Application.UseCases.MachineUC
{
    public class RequestStatusMachineUseCase : IRequestStatusMachineUseCase
    {
        private readonly IMachineSignalRServices _signalR;

        public RequestStatusMachineUseCase(IMachineSignalRServices signalR)
        {
            _signalR = signalR;
        }

        public async Task ExecuteAsync(string equipmentId) => await _signalR.RequestStatusAsync(equipmentId);
    }
}
