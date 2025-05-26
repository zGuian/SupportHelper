using Bogus;
using Moq;
using SupportHelper.Communication.Requests;
using SupportHelper.Communication.Responses;
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
            var hostname = faker.Random.String();
            var ipv4 = faker.Internet.Ip();
            var replyToQueueName = faker.Random.String();
            var request = new MachineInformationRequest(hostname, ipv4, replyToQueueName);
            var response = new Mock<MachineInformationResponse>(hostname, Guid.NewGuid().ToString());
        }
    }
}
