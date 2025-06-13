namespace SupportHelper.WinServices.Application.Interfaces.Services
{
    public interface IMachineService
    {
        Task<bool> MakeAvailableLogSgpClient(string productionLine);
        Task<bool> UpdateSgpClient(string productionLine);
    }
}
