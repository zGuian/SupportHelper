using SupportHelper.WinServices.Core.Models;

namespace SupportHelper.WinServices.Core.Interfaces
{
    public interface IMachineService
    {
        MachineModel GetInformationMachine(CancellationToken cancellationToken = default);
    }
}
