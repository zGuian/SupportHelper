using SupportHelper.Communication.Responses;

namespace SupportHelper.FrontEnd.MVC.Interfaces
{
    public interface IMachineRepository
    {
        Task<ResponseMachine> GetMachineByHostnameAsync(string hostname);
        Task<bool> RequestStatusToMachineAsync(string hostname);
        Task UpdateSgpClientAsync();
    }
}
