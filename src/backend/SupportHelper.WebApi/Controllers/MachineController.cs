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
        [HttpGet("StatusMachine/{hostname:required}")]
        public async Task<IActionResult> GetStatusToMachine([FromServices] IRequestStatusMachineUseCase statusMachineUseCase,
            [FromRoute] string hostname)
        {
            await statusMachineUseCase.ExecuteAsync(hostname);
            return Ok();
        }

        [HttpGet("LogSgpClient/{hostname:required}")]
        public async Task<IActionResult> GetLogsForSgpClientAsync([FromServices] IRequestLogsSgpClientUseCase sgpClientUseCase,
            [FromBody] RequestBase<RequestMachine> request, [FromRoute] string hostname, [FromHeader] string? exchange)
        {
            await sgpClientUseCase.ExecuteAsync(request, new RabbitMQRequest(hostname, exchange));
            return Ok();
        }

        [HttpPost("UpdateSgpCliet")]
        public async Task<IActionResult> UpdateSgpClient([FromServices] IRequestUpdateSgpClientUseCase request,
            [FromBody] RequestUpdateSgpClientJson json)
        {
            await request.ExecuteAsync(json);
            return Ok();
        }

        [HttpGet("GetMachinesConnected")]
        public async Task<IActionResult> GetMachinesConnected([FromServices] IRequestGetAllMachinesUseCase request,
            [FromQuery] int pageNumber, [FromQuery] int pageSize)
        {
            var data = await request.ExecuteAsync(pageNumber, pageSize);
            return Ok(data);
        }
    }
}
