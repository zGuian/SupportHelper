using SupportHelper.Infrastructure.SignalR.Interfaces;
using System.Collections.Concurrent;

namespace SupportHelper.Infrastructure.SignalR.SignalRServices
{
    public class ConnectionService : IConnectionService
    {
        private readonly ConcurrentDictionary<string, string> _map = new();

        public void Register(string equipmentId, string connectionId) 
        {
            _map[equipmentId] = connectionId;
        }

        public string? GetConnectionId(string equipmentId) 
        {
            return _map.TryGetValue(equipmentId, out var connectionId) ? connectionId : null;
        }

        public void Remove(string equipmentId)
        {
            _map.TryRemove(equipmentId, out _);
        }
    }
}
