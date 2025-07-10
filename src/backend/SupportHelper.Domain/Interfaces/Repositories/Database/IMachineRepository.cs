using SupportHelper.Domain.Entities;

namespace SupportHelper.Domain.Interfaces.Repositories.Database
{
    public interface IMachineRepository
    {
        Task<(HashSet<Machine>?, int total)> GetAllMachinesAsync(int pageNumber, int pageSize);
        Task<Machine?> GetMachineAsync(string id);
        Task InsertMachineByProcedure(Machine machine);
        Task UpdateMachineAsync(Machine machine);
    }
}
