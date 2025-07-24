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
            var json = await statusMachineUseCase.ExecuteAsync(hostname.ToLower());
            return Ok(json);
        }

        [HttpGet("LogSgpClient")]
        public async Task<IActionResult> GetLogsForSgpClientAsync([FromServices] IRequestLogsSgpClientUseCase sgpClientUseCase,
            [FromBody] RequestLogsSgpClientJson request)
        {
            await sgpClientUseCase.ExecuteAsync(request);
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
