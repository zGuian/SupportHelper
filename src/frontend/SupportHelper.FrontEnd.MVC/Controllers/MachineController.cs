using Microsoft.AspNetCore.Mvc;
using SupportHelper.FrontEnd.MVC.Interfaces;
using SupportHelper.FrontEnd.MVC.Models;
using SupportHelper.FrontEnd.MVC.Models.ValueObjects;

namespace SupportHelper.FrontEnd.MVC.Controllers
{
    [Route("Maquinas")]
    public class MachineController : Controller
    {
        [HttpGet("ListaDeMaquinas")]
        public async Task<IActionResult> ListOfMachines([FromServices] IMachineServices services, CancellationToken ct = default)
        {
            //var response = await services.GetMachineAsync(ct);
            //ViewBag.SearchModel = new SearchModel();
            //return View(response);

            await Task.Delay(4);

            var machines = new List<MachineModel>();
            var network = new List<NetworkBoardVO>
            {
                NetworkBoardVO.Create("PLACA DE REDE", $"53.93.157.28", "fe80::7074:da19:7a12:d562%13", "ACB480E1FHG", true),
                NetworkBoardVO.Create("PLACA DE REDE", "10.20.20.30", string.Empty, "ACB480E1FEFF", false)
            };
            for (int i = 0; i < 10; i++)
            {
                machines.Add(new MachineModel((i + 1).ToString(), "HOSTNAME", "GUIAN", "TBAD", "WIN 10", network, "10:00:20", "AGORA"));
            }
            ViewBag.SearchModel = new SearchModel();
            var value = ResponseBase<IEnumerable<MachineModel>>.ReturnSuccess(machines);
            return View(value);
        }

        [HttpGet("InfoMaquina/{id}")]
        public async Task<IActionResult> InfoMachine([FromServices] IMachineServices services, [FromRoute] string id)
        {
            var network = new List<NetworkBoardVO>
            {
                NetworkBoardVO.Create("PLACA DE REDE", $"53.93.157.28", "fe80::7074:da19:7a12:d562%13", "ACB480E1FHG", true),
                NetworkBoardVO.Create("PLACA DE REDE", "10.20.20.30", string.Empty, "ACB480E1FEFF", false)
            };
            var machines = new MachineModel("BOLOLOHAHA", "HOSTNAME", "GUIAN", "TBAD", "WIN 10", network, "10:00:20", "AGORA");
            //var response = await services.GetMachineAsync(id);
            return PartialView("_InfoMachine", machines);
        }
    }
}
