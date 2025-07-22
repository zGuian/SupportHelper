using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using SupportHelper.Application.UseCases.MachineUC;
using SupportHelper.Communication.Responses;
using SupportHelper.Test.Common.Utilities.Repositories;
using SupportHelper.Test.Common.Utilities.Requests;
using SupportHelper.Test.Common.Utilities.Services;

namespace SupportHelper.Tests.UserCasesTest.MachineUseCase
{
    public class RequestMachineInformationUseCaseTest
    {
        [Fact]
        public async Task Success()
        {
            var request = RequestStatusMachineJsonBuilder.Build();
            var json = RequestStatusMachineJsonBuilder.Build();
            var useCase = CreateUseCase();
            var responseJson = new ResponseStatusMachineJson
            {
                Hostname = request.Hostname,
                CurrentUsername = "user",
                DomainName = "domain",
                IsConnected = true,
                LastUpdate = "2024-06-01",
                OperationalSystem = "Windows",
                UpTime = "10:00"
            };

            await useCase.ExecuteAsync(request.Hostname);

            // Act
            var result = await useCase.ExecuteAsync(request.Hostname);

            // Assert
            Assert.Equal(responseJson, result);
        }

        private static RequestStatusMachineUseCase CreateUseCase()
        {
            var log = new NullLogger<RequestMachineInformationUseCase>();
            var signalRService = MachineSignalRServiceBuilder.Build();
            var machineRepository = MachineRepositoryBuilder.Build();
            return new RequestStatusMachineUseCase(signalRService, machineRepository);
        }
    }
}
