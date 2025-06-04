using SupportHelper.Domain.Entities;

namespace SupportHelper.Domain.Interfaces.Repositories
{
    public interface IMachineRepository
    {
        Task<Machine> GetMachineAsync(Guid id);
    }
}
