using SupportHelper.FrontEnd.MVC.Models;

namespace SupportHelper.FrontEnd.MVC.Interfaces
{
    public interface IMachineServices
    {
        Task<MachineModel> GetMachineByHostnameAsync(string hostname);
    }
}
