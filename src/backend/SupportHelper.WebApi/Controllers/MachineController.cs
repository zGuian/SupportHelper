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
            var json = await statusMachineUseCase.ExecuteAsync(hostname.ToLower().Trim(), cancellationToken);
            return Ok(new
            {
                IsSuccess = true,
                OnDate = DateTime.Now.ToString("dd/MM/yyyy | hh:mm"),
                Data = json
            });
        }

        [HttpGet("{hostname:required}")]
        public async Task<IActionResult> GetInformationMachineAsync([FromServices] IMachineInformationUseCase useCase,
            [FromRoute] string hostname, CancellationToken cancellationToken)
        {
            var json = await useCase.ExecuteAsync(hostname.ToLower().Trim(), cancellationToken);
            return Ok(new
            {
                IsSuccess = true,
                OnDate = DateTime.Now.ToString("dd/MM/yyyy | hh:mm"),
                Data = json
            });
        }

        [HttpPost("LogSgpClient")]
        public async Task<IActionResult> GetLogsForSgpClientAsync([FromServices] ILogsSgpClientUseCase logsSgpClientUseCase,
            [FromBody] RequestLogsSgpClientJson request, CancellationToken cancellationToken)
        {
            await logsSgpClientUseCase.ExecuteAsync(request, cancellationToken);
            return Ok(new
            {
                IsSuccess = true,
                OnDate = DateTime.Now.ToString("dd/MM/yyyy | hh:mm"),
            });
        }

        [HttpPost("UpdateSgpCliet")]
        public async Task<IActionResult> UpdateSgpClient([FromServices] IUpdateSgpClientUseCase updateSgpClientUseCase,
        [FromBody] RequestUpdateSgpClientJson request, CancellationToken cancellationToken)
        {
            await updateSgpClientUseCase.ExecuteAsync(request, cancellationToken);
            return Ok(new
            {
                IsSuccess = true,
                OnDate = DateTime.Now.ToString("dd/MM/yyyy | hh:mm"),
            });
        }

        [HttpGet("GetMachinesConnected")]
        public async Task<IActionResult> GetMachinesConnected([FromServices] IGetAllMachinesActivesUseCase getAllMachinesUseCase)
        {
            var json = await getAllMachinesUseCase.ExecuteAsync();
            return Ok(new
            {
                IsSuccess = true,
                OnDate = DateTime.Now.ToString("dd/MM/yyyy | hh:mm"),
                Data = json
            });
        }
    }
}
