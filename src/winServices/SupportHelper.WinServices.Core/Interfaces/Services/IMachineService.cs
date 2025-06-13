using SupportHelper.WinServices.Core.Models;

namespace SupportHelper.WinServices.Core.Interfaces.Services
{
    public interface IMachineService
    {
        MachineModel GetInformationMachine(CancellationToken cancellationToken = default);
        Task<bool> MakeAvailableLogSgpClient(string productionLine);
        Task<bool> UpdateSgpClient(string productionLine);
    }
}
