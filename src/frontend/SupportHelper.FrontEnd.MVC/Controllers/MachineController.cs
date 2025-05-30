using Microsoft.AspNetCore.Mvc;
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
            for (int i = 1; i < 50; i++)
            {
                machines.Add(new MachineModel(Guid.NewGuid().ToString(), "BOLOLO", "GUIAN", "EMEA", "WIN 10"));
            }
            return View(machines);
        }
    }
}
