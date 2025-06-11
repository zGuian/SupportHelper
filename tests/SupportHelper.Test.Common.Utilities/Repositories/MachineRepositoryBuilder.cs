using Moq;
using SupportHelper.Domain.Interfaces.Repositories;

namespace SupportHelper.Test.Common.Utilities.Repositories
{
    public class MachineRepositoryBuilder
    {
        public static IMachineRepository Build()
        {
            var mock = new Mock<IMachineRepository>();
            return mock.Object;
        }
    }
}
