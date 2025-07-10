namespace SupportHelper.WinServices.Application.Interfaces.Services
{
    public interface IMachineService
    {
        Task<bool> MakeAvailableLogSgpClient(string productionLine);
        Task<string> UpdateSgpClient(string productionLine);
    }
}
