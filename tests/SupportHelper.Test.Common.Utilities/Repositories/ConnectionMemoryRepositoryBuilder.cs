using Moq;
using SupportHelper.Domain.Interfaces.Repositories.Memory;

namespace SupportHelper.Test.Common.Utilities.Repositories
{
    public class ConnectionMemoryRepositoryBuilder
    {
        public static IConnectionMemoryRepository Build()
        {
            var mock = new Mock<IConnectionMemoryRepository>();
            return mock.Object;
        }
    }
}
