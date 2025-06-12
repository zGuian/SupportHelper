using SupportHelper.WinServices.Core.Models;
using SupportHelper.WinServices.Core.Models.Enums;

namespace SupportHelper.WinServices.Core.Interfaces.Services
{
    public interface IMachineService
    {
        MachineModel GetInformationMachine(CancellationToken cancellationToken = default);
        void MakeAvailableLogSgpClient(SGPClientLine productionLine);
    }
}
