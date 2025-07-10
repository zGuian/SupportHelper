using SupportHelper.Communication.Responses;

namespace SupportHelper.Infrastructure.Data.Interfaces
{
    public interface IMachineServices
    {
        Task SaveInDatabaseAndInMemory(ResponseStatusMachineJson response);
        Task UpdateDatabaseAsync(ResponseStatusMachineJson response);
    }
}
