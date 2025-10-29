using SupportHelper.Service.Domain.DTOs.Requests;

namespace SupportHelper.Service.Domain.Interface.Services
{
    public interface IMachineServices
    {
        Task<string> GetStatusToMachine();
        Task<string> MakeAvailableLogSgpClient(RequestLogsSgpClient request);
        Task<string> MakeAvailableLogSgpClient(string productionLine, string requestId);
        Task<string> UpdateSgpClient(string productionLine);
    }
}
