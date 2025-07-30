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
        public async Task<IActionResult> GetLogsForSgpClientAsync([FromServices] ILogsSgpClientUseCase logsSgpClientUseCase,
            [FromBody] RequestLogsSgpClientJson request)
        {
            await logsSgpClientUseCase.ExecuteAsync(request);
            return Ok();
        }

        [HttpPost("UpdateSgpCliet")]
        public async Task<IActionResult> UpdateSgpClient([FromServices] IUpdateSgpClientUseCase updateSgpClientUseCase,
            [FromBody] RequestUpdateSgpClientJson request)
        {
            await updateSgpClientUseCase.ExecuteAsync(request);
            return Ok();
        }

        [HttpGet("GetMachinesConnected")]
        public async Task<IActionResult> GetMachinesConnected([FromServices] IGetAllMachinesUseCase getAllMachinesUseCase,
            [FromQuery] int pageNumber, [FromQuery] int pageSize)
        {
            var data = await getAllMachinesUseCase.ExecuteAsync(pageNumber, pageSize);
            return Ok(data);
        }
    }
}
