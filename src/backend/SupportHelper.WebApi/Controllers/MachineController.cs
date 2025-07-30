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
        public async Task<IActionResult> GetStatusToMachine([FromServices] IStatusMachineUseCase statusMachineUseCase,
            [FromRoute] string hostname, CancellationToken cancellationToken)
        {
            var json = await statusMachineUseCase.ExecuteAsync(hostname.ToLower(), cancellationToken);
            return Ok(json);
        }

        [HttpPost("LogSgpClient")]
        public async Task<IActionResult> GetLogsForSgpClientAsync([FromServices] ILogsSgpClientUseCase sgpClientUseCase,
            [FromBody] RequestLogsSgpClientJson request)
        {
            await sgpClientUseCase.ExecuteAsync(request);
            return Ok();
        }

        [HttpPost("UpdateSgpCliet")]
        public async Task<IActionResult> UpdateSgpClient([FromServices] IUpdateSgpClientUseCase request,
            [FromBody] RequestUpdateSgpClientJson json)
        {
            await request.ExecuteAsync(json);
            return Ok();
        }

        [HttpGet("GetMachinesConnected")]
        public async Task<IActionResult> GetMachinesConnected([FromServices] IGetAllMachinesUseCase request,
            [FromQuery] int pageNumber, [FromQuery] int pageSize)
        {
            var data = await request.ExecuteAsync(pageNumber, pageSize);
            return Ok(data);
        }
    }
}
