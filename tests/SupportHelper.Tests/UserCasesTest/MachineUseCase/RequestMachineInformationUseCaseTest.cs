using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using SupportHelper.Application.UseCases.MachineUC;
using SupportHelper.Domain.Interfaces.MQServices;
using SupportHelper.Test.Common.Utilities.Requests;
using SupportHelper.Test.Common.Utilities.Services;

namespace SupportHelper.Tests.UserCasesTest.MachineUseCase
{
    public class RequestMachineInformationUseCaseTest
    {
        [Fact]
        public async Task Success()
        {
            var request = MachineInformationRequestBuilder.Build();
            var mockMqService = new Mock<IMachineMQServices>();
            var useCase = CreateUseCase();
            mockMqService.Setup(x => x.PublishMessageAsync(request));

            await useCase.ExecuteAsync(request);

            mockMqService.Verify(x => x.PublishMessageAsync(request), Times.Once);
        }

        private static RequestMachineInformationUseCase CreateUseCase()
        {
            var log = new NullLogger<RequestMachineInformationUseCase>();
            var mqService = MachineMQServiceBuilder.Build();
            var signalRService = MachineSignalRServiceBuilder.Build();
            return new RequestMachineInformationUseCase(log, mqService, signalRService);
        }
    }
}
