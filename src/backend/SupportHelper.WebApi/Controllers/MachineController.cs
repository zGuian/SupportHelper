using Microsoft.AspNetCore.Mvc;
using SupportHelper.Application.Interfaces;
using SupportHelper.Communication.Requests;

namespace SupportHelper.WebApi.Controllers
{
    [ApiController]
    [Route("api/{version}/[controller]")]
    public sealed class MachineController : ControllerBase
    {
        [HttpGet("Information/{hostname}")]
        public async Task<IActionResult> GetMachineInformation([FromServices] IRequestMachineInformation getMachineInformation,
            [FromRoute] string hostname, [FromHeader] string nameQueueResponse, [FromHeader] string exchange)
        {
            var request = new MachineInformationRequest(new RabbitMQRequest(
                                                        hostname, 
                                                        exchange, 
                                                        nameQueueResponse));

            await getMachineInformation.ExecuteAsync(request);
            return Ok();
        }
    }
}
