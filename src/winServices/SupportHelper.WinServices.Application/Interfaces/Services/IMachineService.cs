namespace SupportHelper.WinServices.Application.Interfaces.Services
{
    public interface IMachineService
    {
        Task<bool> MakeAvailableLogSgpClient(string productionLine, string requestId);
        Task<string> UpdateSgpClient(string productionLine);
    }
}
