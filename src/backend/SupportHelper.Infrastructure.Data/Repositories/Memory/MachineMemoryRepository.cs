using SupportHelper.Domain.Entities;
using SupportHelper.Domain.Interfaces.Repositories.Memory;
using System.Collections.Concurrent;

namespace SupportHelper.Infrastructure.Data.Repositories.Memory
{
    public class MachineMemoryRepository : IMachineMemoryRepository
    {
        private ConcurrentDictionary<string, Machine> _map = new();
        private List<string> _connIds = new();

        public void Register(string id, Machine machine)
        {
            _connIds.Add(id);
            _map[id] = machine;
        }

        public Machine? GetMachineById(string id) 
            => _map.TryGetValue(id, out var machine) ? machine : null;

        public string GetConnectionId(string id)
        {
            return _connIds.FirstOrDefault(x => x.Equals(id) ?? throw new Exception("NÃO FOI ENCONTRADO CHAVE DE CONEXÃO"));
        }

        public void Remove(string equipmentId) 
            => _map.TryRemove(equipmentId, out _);
    }
}
