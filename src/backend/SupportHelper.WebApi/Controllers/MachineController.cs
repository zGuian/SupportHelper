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
            [FromRoute] string hostname)
        {
            var request = new RequestMachineInformationJson { Hostname =  hostname };
            await getMachineInformation.ExecuteAsync(request);
            return Ok();
        }

        [HttpGet("LogSgpClient/{hostname:required}")]
        public async Task<IActionResult> GetLogsForSgpClientAsync([FromServices] IRequestLogsSgpClientUseCase sgpClientUseCase,
            [FromBody] RequestBase<RequestMachine> request, [FromRoute] string hostname, [FromHeader] string? exchange)
        {
            await sgpClientUseCase.ExecuteAsync(request, new RabbitMQRequest(hostname, exchange));
            return Ok();
        }

        [HttpGet("StatusMachine/{hostname:required}")]
        public async Task<IActionResult> GetStatusToMachine([FromServices] IRequestStatusMachineUseCase statusMachineUseCase,
            [FromRoute] string hostname)
        {
            await statusMachineUseCase.ExecuteAsync(hostname);
            return Ok();
        }

        [HttpPost("UpdateSgpCliet")]
        public async Task<IActionResult> UpdateSgpClient([FromServices] IRequestUpdateSgpClientUseCase request,
            [FromBody] RequestUpdateSgpClientJson json)
        {
            await request.ExecuteAsync(json);
            return Ok();
        }
    }
}
