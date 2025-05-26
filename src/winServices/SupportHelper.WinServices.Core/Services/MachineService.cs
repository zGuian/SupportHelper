using SupportHelper.WinServices.Core.Interfaces;
using SupportHelper.WinServices.Core.Models;

namespace SupportHelper.WinServices.Core.Services
{
    public sealed class MachineService : IMachineService
    {
        private readonly IMQServicesProducer _mqServices;

        public MachineService(IMQServicesProducer services)
        {
            _mqServices = services;
        }

        public async Task GetInformationFromMachine()
        {
            var machine = new MachineModel();
            machine.GetAllInformationFromMachine();
            await _mqServices.SendMachineByMessageAsync(machine);
        }
    }
}
