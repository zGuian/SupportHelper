using Microsoft.AspNetCore.Mvc;
using SupportHelper.API.Domain.DTOs.Entities;
using SupportHelper.API.Domain.DTOs.Requests;
using SupportHelper.API.Domain.DTOs.Responses;
using SupportHelper.API.Domain.Interfaces.Repositories;
using SupportHelper.API.Domain.Interfaces.Services;

namespace SupportHelper.API.WebApi.Controllers
{
    [ApiController]
    [Route("api/v1/Machine")]
    public class MachineController(IMachineServices machineServices) : ControllerBase
    {
        private readonly IMachineServices _machineServices = machineServices;

        [HttpGet("ObtemStatus/{hostname:required}")]
        [EndpointSummary("Realiza comunicação com client, coleta as informações atuais e atualiza no banco de dados.")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseStatusMachineJson))]
        //[ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ResponseErrorJson))]
        //[ProducesErrorResponseType(typeof(ResponseErrorJson))]
        public async Task<IActionResult> GetStatusToMachine([FromRoute] string hostname, CancellationToken ct)
        {
            var dto = await _machineServices.GetInformationAndUpdateDatabaseAsync(hostname.ToLower().Trim(), true, ct);
            return Ok(new
            {
                IsSuccess = true,
                OnDate = DateTime.Now.ToString("dd/MM/yyyy-hh:mm:ss"),
                Data = dto
            });
        }

        [HttpGet("ObtemInformacao/{hostname:required}")]
        [EndpointSummary("Obtém informação cadastrada no banco de dados.")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(MachineDto))]
        //[ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ResponseErrorJson))]
        //[ProducesErrorResponseType(typeof(ResponseErrorJson))]
        public async Task<IActionResult> GetInformationMachineAsync([FromRoute] string hostname
            , CancellationToken ct)
        {
            var json = await _machineServices.GetInformationMachineInDatabaseAsync(hostname.ToLower().Trim(), ct);
            return Ok(new
            {
                IsSuccess = true,
                OnDate = DateTimeOffset.Now.LocalDateTime,
                Data = json
            });
        }

        [HttpPost("ObtemLogsDoSgpClient")]
        [EndpointSummary("Busca pasta [log] do SGP Client e disponibiliza um download .zip")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ResponseErrorJson))]
        //[ProducesErrorResponseType(typeof(ResponseErrorJson))]
        public async Task<IActionResult> GetLogsForSgpClientAsync([FromBody] RequestLogsSgpClientJson request
            , CancellationToken ct)
        {
            var fileDto = await _machineServices.GetLogsSgpClientAsync(request, ct);
            if (fileDto == null || fileDto.HasData == false) return NotFound("ARQUIVOS NÃO ENCONTRADOS");
            return File(fileDto.Stream, fileDto.ContentType, fileDto.FileName, false);
        }

        [HttpPost("AtualizaSgpClient")]
        [EndpointSummary("Fecha e abre o SGP Client. Iniciado pelo START forçando atualização.")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseUpdateSgpClientJson))]
        //[ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ResponseErrorJson))]
        //[ProducesErrorResponseType(typeof(ResponseErrorJson))]
        public async Task<IActionResult> UpdateSgpClient([FromBody] RequestUpdateSgpClientJson request
            , CancellationToken ct)
        {
            var json = await _machineServices.UpdateOnlySgpClientAsync(request, ct);
            return Ok(new
            {
                IsSuccess = true,
                OnDate = DateTime.Now.ToString("dd/MM/yyyy-hh:mm:ss"),
                Data = json
            });
        }

        [HttpPost("AtualizaMuitosSgpClient")]
        [EndpointSummary(@"Envia a uma fila para processar e iniciar processo de fecha e abre o SGP Client. 
                           Iniciado pelo START forçando atualização.")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<ResponseBase<ResponseUpdateSgpClientJson>>))]
        //[ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ResponseErrorJson))]
        public async Task<IActionResult> UpdateSgpClient([FromBody] IEnumerable<RequestUpdateSgpClientJson> request
            , CancellationToken ct)
        {
            var json = await _machineServices.UpdateManySgpClientAsync(request, ct);
            return Ok(new
            {
                IsSuccess = true,
                OnDate = DateTime.Now.ToString("dd/MM/yyyy-hh:mm:ss"),
                Data = json
            });
        }

        [HttpPost("ReceivedFiles")]
        [EndpointSummary(@"Recebe pasta .zip contendo log do SGP Client")]
        public async Task<IActionResult> ReceivedFiles([FromServices] ICacheTemp cache
            , [FromHeader(Name = "Id-Request")] string requestId
            , IFormFile folderZip)
        {
            if (string.IsNullOrEmpty(requestId))
                return BadRequest("Id-Request ausente.");

            await using var ms = new MemoryStream();
            await folderZip.CopyToAsync(ms);

            await cache.StoreFileAsync(requestId, ms.ToArray(), folderZip.ContentType, TimeSpan.FromMinutes(2));

            return Ok();
        }
    }
}
