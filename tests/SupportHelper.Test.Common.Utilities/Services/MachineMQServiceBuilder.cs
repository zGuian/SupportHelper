using Moq;
using SupportHelper.Domain.Interfaces.MQServices;

namespace SupportHelper.Test.Common.Utilities.Services
{
    public class MachineMQServiceBuilder
    {
        public static IMachineMQServices Build()
        {
            var mock = new Mock<IMachineMQServices>();
            return mock.Object;
        }
    }
}
