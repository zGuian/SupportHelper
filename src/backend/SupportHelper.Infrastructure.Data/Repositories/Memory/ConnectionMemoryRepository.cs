using SupportHelper.Domain.Interfaces.Repositories.Memory;
using System.Collections.Concurrent;

namespace SupportHelper.Infrastructure.Data.Repositories.Memory
{
    public class ConnectionMemoryRepository : IConnectionMemoryRepository
    {
        private readonly ConcurrentDictionary<string, string> _map = new();

        public void Register(string equipmentId, string connectionId)
            => _map[equipmentId] = connectionId;

        public string? GetConnectionId(string equipmentId)
         => _map.TryGetValue(equipmentId, out var connectionId) ? connectionId : null;

        public void Remove(string equipmentId)
            => _map.TryRemove(equipmentId, out _);
    }
}
