using Bogus;
using Moq;
using SupportHelper.WebApi.Controllers;

namespace SupportHelper.Tests.ControllersTest
{
    public class MachineControllerTest
    {
        private readonly Mock<MachineController> _machineControllerMock;

        public MachineControllerTest()
        {
            _machineControllerMock = new Mock<MachineController>();
        }

        [Fact]
        public async Task GetMachineInformation_ShouldBeMachine()
        {
            var faker = new Faker("pt-BR");
        }
    }
}
