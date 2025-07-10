using Microsoft.Extensions.Logging.Abstractions;
using SupportHelper.Application.UseCases.MachineUC;
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

            await useCase.ExecuteAsync(request.Hostname);

            // FALTA TERMINAR
        }

        private static RequestStatusMachineUseCase CreateUseCase()
        {
            var log = new NullLogger<RequestMachineInformationUseCase>();
            var signalRService = MachineSignalRServiceBuilder.Build();
            var connMemoryRepository = ConnectionMemoryRepositoryBuilder.Build();
            return new RequestStatusMachineUseCase(signalRService, connMemoryRepository);
        }
    }
}
