using SupportHelper.FrontEnd.MVC.Models;

namespace SupportHelper.FrontEnd.MVC.Interfaces
{
    public interface IMachineServices
    {
        Task<ResponseBase<MachineModel>> GetMachineAsync(string hostname, CancellationToken ct = default);
        Task<ResponseBase<IEnumerable<MachineModel>>> GetMachineAsync(CancellationToken ct = default);
    }
}
