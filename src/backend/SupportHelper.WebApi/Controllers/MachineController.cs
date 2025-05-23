using Microsoft.AspNetCore.Mvc;
using SupportHelper.Application.Interfaces;
using SupportHelper.Communication.Requests;

namespace SupportHelper.WebApi.Controllers
{
    [ApiController]
    [Route("api/{version}/[controller]")]
    public sealed class MachineController : ControllerBase
    {
        [HttpGet("Information")]
        public async Task<IActionResult> GetMachineInformation([FromServices] IGetMachineInformation getMachineInformation,
            MachineInformationRequest request)
        {
            var response = await getMachineInformation.ExecuteAsync(request);
            return Ok(response);
        }
    }
}
