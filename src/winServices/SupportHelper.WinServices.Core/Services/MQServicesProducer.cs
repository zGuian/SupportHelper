using SupportHelper.WinServices.Core.Interfaces;
using SupportHelper.WinServices.Core.Models;

namespace SupportHelper.WinServices.Core.Services
{
    public sealed class MQServicesProducer : IMQServicesProducer
    {
        public Task SendMachineByMessageAsync(MachineModel machine)
        {
            throw new NotImplementedException();
        }
    }
}
