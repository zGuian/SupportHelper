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
        [HttpGet("Information/{hostname}")]
        public async Task<IActionResult> GetMachineInformation([FromServices] IRequestMachineInformation getMachineInformation,
            [FromRoute] string hostname, [FromHeader] string? exchange)
        {
            var request = new MachineInformationRequest("GET_INFORMATION_MACHINE",
                                                        new RabbitMQRequest(hostname, exchange));

            await getMachineInformation.ExecuteAsync(request);
            return Ok();
        }
    }
}
