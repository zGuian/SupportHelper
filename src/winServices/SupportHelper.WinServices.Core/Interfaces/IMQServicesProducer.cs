using SupportHelper.WinServices.Core.Models;

namespace SupportHelper.WinServices.Core.Interfaces
{
    public interface IMQServicesProducer
    {
        public Task SendMachineByMessageAsync(MachineModel machine);
    }
}
