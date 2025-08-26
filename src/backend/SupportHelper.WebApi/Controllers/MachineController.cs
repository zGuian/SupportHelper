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
            [FromRoute] string hostname, CancellationToken ct)
        {
            var json = await statusMachineUseCase.ExecuteAsync(hostname.ToLower().Trim(), ct);
            return Ok(new
            {
                IsSuccess = true,
                OnDate = DateTime.Now.ToString("dd/MM/yyyy | hh:mm"),
                Data = json
            });
        }

        [HttpGet("{hostname:required}")]
        public async Task<IActionResult> GetInformationMachineAsync([FromServices] IMachineInformationUseCase useCase,
            [FromRoute] string hostname, CancellationToken ct)
        {
            var json = await useCase.ExecuteAsync(hostname.ToLower().Trim(), ct);
            return Ok(new
            {
                IsSuccess = true,
                OnDate = DateTime.Now.ToString("dd/MM/yyyy-hh:mm:ss"),
                Data = json
            });
        }

        [HttpPost("LogSgpClient")]
        public async Task<IActionResult> GetLogsForSgpClientAsync([FromServices] ILogsSgpClientUseCase logsSgpClientUseCase,
            [FromBody] RequestLogsSgpClientJson request, CancellationToken ct)
        {
            await logsSgpClientUseCase.ExecuteAsync(request, ct);
            return Ok(new
            {
                IsSuccess = true,
                OnDate = DateTime.Now.ToString("dd/MM/yyyy-hh:mm:ss"),
            });
        }

        [HttpPost("UpdateSgpCliet")]
        public async Task<IActionResult> UpdateSgpClient([FromServices] IUpdateSgpClientUseCase updateSgpClientUseCase,
        [FromBody] RequestUpdateSgpClientJson request, CancellationToken ct)
        {
            await updateSgpClientUseCase.ExecuteAsync(request, ct);
            return Ok(new
            {
                IsSuccess = true,
                OnDate = DateTime.Now.ToString("dd/MM/yyyy-hh:mm:ss"),
            });
        }

        [HttpGet("GetMachinesConnected")]
        public async Task<IActionResult> GetMachinesConnected([FromServices] IGetAllMachinesActivesUseCase getAllMachinesUseCase,
            CancellationToken ct)
        {
            var json = await getAllMachinesUseCase.ExecuteAsync(ct);
            return Ok(new
            {
                IsSuccess = true,
                OnDate = DateTime.Now.ToString("dd/MM/yyyy-hh:mm:ss"),
                Data = json
            });
        }
    }
}
