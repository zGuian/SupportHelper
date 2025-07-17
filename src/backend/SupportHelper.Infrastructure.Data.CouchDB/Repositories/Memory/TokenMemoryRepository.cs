using SupportHelper.Domain.Interfaces.Repositories.Memory;
using System.Collections.Concurrent;

namespace SupportHelper.Infrastructure.Data.CouchDB.Repositories.Memory
{
    public sealed class TokenMemoryRepository : ITokenMemoryRepository
    {
        private ConcurrentDictionary<string, string> _map = new();

        public (string user, string token) GetToken(string userId)
        {
            throw new NotImplementedException();
        }

        public void RegisterToken(string userId, string token)
        {
            throw new NotImplementedException();
        }

        public void RemoveToken(string userId)
        {
            throw new NotImplementedException();
        }
    }
}
