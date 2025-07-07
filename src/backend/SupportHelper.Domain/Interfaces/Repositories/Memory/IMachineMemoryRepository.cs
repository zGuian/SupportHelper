using SupportHelper.Domain.Entities;

namespace SupportHelper.Domain.Interfaces.Repositories.Memory
{
    public interface IMachineMemoryRepository
    {
        Machine? GetConnectionId(string id);
        public void Register(string id, Machine machine);
        void Remove(string equipmentId);
    }
}
