using SupportHelper.Communication.Responses;
using SupportHelper.Infrastructure.SignalR.Interfaces;
using SupportHelper.Infrastructure.Data.Interfaces;

namespace SupportHelper.Infrastructure.SignalR.Hubs
{
    public class ControlHub : BaseHub
    {
        private readonly IMachineServices _machineServices;

        public ControlHub(IConnectionService connectionService, IMachineServices machineServices) :
            base(connectionService) => _machineServices = machineServices;

        public async Task ResponseStatusMachine(MachineInformationResponse response)
        {
            await _machineServices.SaveInDatabaseAndInMemory(response);
        }
    }
}
