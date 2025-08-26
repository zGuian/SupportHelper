using Microsoft.AspNetCore.Mvc;
using Moq;
using SupportHelper.Application.Interfaces;
using SupportHelper.Communication.Responses;
using SupportHelper.WebApi.Controllers;

namespace SupportHelper.Tests.ControllersTest
{
    public class MachineControllerTest
    {
        [Fact]
        public async Task GetStatusToMachine_ShouldBeOk()
        {
            var moq = new Mock<IStatusMachineUseCase>();
            moq.Setup(x => x.ExecuteAsync("test", default))
                .ReturnsAsync(new ResponseStatusMachineJson 
                { 
                    Id = Guid.NewGuid().ToString(),
                    Hostname = "test", 
                    CurrentUsername = "test", 
                    DomainName = "test", 
                    IsConnected = true, 
                    LastUpdate = "test", 
                    OperationalSystem = "test", 
                    UpTime = "test" 
                });

            var controller = new MachineController();
            var hostname = "test";
            
            var result = await controller.GetStatusToMachine(moq.Object, hostname, default);
            
            Assert.IsType<OkObjectResult>(result);
        }
    }
}
