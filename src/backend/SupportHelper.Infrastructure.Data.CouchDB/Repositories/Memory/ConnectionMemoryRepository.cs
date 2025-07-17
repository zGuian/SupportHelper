using SupportHelper.Domain.Interfaces.Repositories.Memory;
using SupportHelper.Exceptions.ExceptionsBase;
using System.Collections.Concurrent;

namespace SupportHelper.Infrastructure.Data.CouchDB.Repositories.Memory
{
    public sealed class ConnectionMemoryRepository : IConnectionMemoryRepository
    {
        private readonly ConcurrentDictionary<string, string> _map = new();

        public void GetConnectionId(string hostname, out string connId)
        {
            _map.TryGetValue(hostname, out string? connectionId);
            connId = connectionId ?? string.Empty;
        }

        public void Register(string hostname, string connectionId) 
            => _map[hostname] = connectionId;

        public void Remove(string hostname)
            => _map.TryRemove(hostname, out _);
    }
}
