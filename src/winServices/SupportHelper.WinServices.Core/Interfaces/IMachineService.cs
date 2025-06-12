using SupportHelper.WinServices.Core.Models;
using SupportHelper.WinServices.Core.Models.Enums;

namespace SupportHelper.WinServices.Core.Interfaces
{
    public interface IMachineService
    {
        MachineModel GetInformationMachine(CancellationToken cancellationToken = default);
        void MakeAvailableLogSgpClient(SGPClientLine productionLine);
    }
}
