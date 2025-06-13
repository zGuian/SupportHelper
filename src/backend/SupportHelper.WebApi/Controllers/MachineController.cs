using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using SupportHelper.Application.Interfaces;
using SupportHelper.Communication.Requests;

namespace SupportHelper.WebApi.Controllers
{
    [ApiController]
    [ApiVersion(1.0)]
    [Route("api/v{v:apiVersion}/[controller]")]
    public sealed class MachineController : ControllerBase
    {
        [HttpGet("Information/{hostname:required}")]
        public async Task<IActionResult> GetMachineInformation([FromServices] IRequestMachineInformationUseCase getMachineInformation,
            [FromRoute] string hostname, [FromHeader] string? exchange)
        {
            var request = new MachineInformationRequest("GET_INFORMATION_MACHINE",
                                                        new RabbitMQRequest(hostname, exchange));

            await getMachineInformation.ExecuteAsync(request);
            return Ok();
        }

        [HttpPost("LogSgpClient/{hostname:required}")]
        public async Task<IActionResult> GetLogsForSgpClientAsync([FromServices] IRequestLogsSgpClientUseCase sgpClientUseCase,
            [FromBody] RequestBase<RequestMachine> request, [FromRoute] string hostname, [FromHeader] string? exchange)
        {
            await sgpClientUseCase.ExecuteAsync(request, new RabbitMQRequest(hostname, exchange));
            return Ok();
        }
    }
}
