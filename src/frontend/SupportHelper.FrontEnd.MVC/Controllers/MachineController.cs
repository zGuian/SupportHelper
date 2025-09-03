using Microsoft.AspNetCore.Mvc;
using SupportHelper.FrontEnd.MVC.Interfaces;
using SupportHelper.FrontEnd.MVC.Models;

namespace SupportHelper.FrontEnd.MVC.Controllers
{
    [Route("Maquinas")]
    public class MachineController : Controller
    {
        [HttpGet("ListaDeMaquinas")]
        public IActionResult ListOfMachines()
        {
            var machines = new List<MachineModel>();
            for (int i = 0; i < 50; i++)
            {
                machines.Add(new MachineModel(i.ToString(), "HOSTNAME", "GUIAN", "TBAD", "WIN 10", null,"10:00:20", "AGORA"));
            }
            ViewBag.Machine = new MachineModel();
            return View(machines);
        }

        [HttpGet("Info")]
        public async Task<IActionResult> InfoMachine([FromServices] IMachineServices services, string hostname)
        {
            var data = await services.GetMachineByHostnameAsync(hostname);
            return View(data);
        }
    }
}
