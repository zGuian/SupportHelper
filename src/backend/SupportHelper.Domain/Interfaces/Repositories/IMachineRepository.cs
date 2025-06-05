using SupportHelper.Domain.Entities;

namespace SupportHelper.Domain.Interfaces.Repositories
{
    public interface IMachineRepository
    {
        Task<HashSet<Machine>> GetAllMachinesAsync(int pageSize, int count);
        Task<Machine?> GetMachineAsync(string id);
        Task InsertMachineByProcedure(Machine machine);
        Task UpdateMachineAsync(Machine machine);
    }
}
