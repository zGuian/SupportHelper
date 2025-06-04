using SupportHelper.Domain.Entities;
using SupportHelper.Domain.Interfaces.Repositories;

namespace SupportHelper.Infrastructure.Persistence
{
    public class MachineRepository : IMachineRepository
    {
        public Task<HashSet<Machine>> GetAllMachinesAsync(int pageSize, int count)
        {
            throw new NotImplementedException();
        }

        public Task<Machine> GetMachineAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateMachineAsync(Machine machine)
        {
            throw new NotImplementedException();
        }
    }
}
