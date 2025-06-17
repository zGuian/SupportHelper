using Moq;
using SupportHelper.Domain.Interfaces.MQServices;
using SupportHelper.Domain.Interfaces.SignalRContext;

namespace SupportHelper.Test.Common.Utilities.Services
{
    public class MachineSignalRServiceBuilder
    {
        public static IMachineSignalRServices Build()
        {
            var mock = new Mock<IMachineSignalRServices>();
            return mock.Object;
        }
    }
}
