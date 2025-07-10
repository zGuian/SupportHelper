using SupportHelper.Domain.Entities;
using SupportHelper.Domain.Interfaces.Repositories.Memory;
using System.Collections.Concurrent;

namespace SupportHelper.Infrastructure.Data.Repositories.Memory
{
    public class MachineMemoryRepository : IMachineMemoryRepository
    {
        private ConcurrentDictionary<string, Machine> _map = new();
        private ConcurrentBag<string> _connId = new();

        public void Register(string id, Machine machine) 
            => _map[id] = machine;

        public Machine? GetMachineById(string id) 
            => _map.TryGetValue(id, out var machine) ? machine : null;

        public void Remove(string equipmentId) 
            => _map.TryRemove(equipmentId, out _);
    }
}
