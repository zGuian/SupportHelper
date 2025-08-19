using SupportHelper.Domain.Interfaces.Repositories.Memory;
using System.Collections.Concurrent;

namespace SupportHelper.Infrastructure.Data.CouchDB.Repositories.Memory
{
    public sealed class TokenMemoryRepository : ITokenMemoryRepository
    {
        private ConcurrentDictionary<string, string> _map = new();

        public void GetToken(string userId, out string token)
        {
            _map.TryGetValue(userId, out string? tokenTemp);
            token = tokenTemp ?? string.Empty;
        }

        public void RegisterToken(string userId, string token)
        {
            _map[userId] = token;
        }

        public void RemoveToken(string userId)
        {
            _map.TryRemove(userId, out string? _);
        }
    }
}
