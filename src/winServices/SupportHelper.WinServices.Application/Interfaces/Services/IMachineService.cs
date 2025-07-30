using SupportHelper.Communication.Requests;

namespace SupportHelper.WinServices.Application.Interfaces.Services
{
    public interface IMachineService
    {
        Task<string> MakeAvailableLogSgpClient(string productionLine, string DestinyArchive);
        Task<string> MakeAvailableLogSgpClient(RequestLogsSgpClientJson request);
        Task<string> UpdateSgpClient(string productionLine);
    }
}
