using SupportHelper.Domain.Interfaces.Repositories.Memory;
using SupportHelper.Exceptions.ExceptionsBase;
using System.Collections.Concurrent;

namespace SupportHelper.Infrastructure.Data.Repositories.Memory
{
    public class ConnectionMemoryRepository : IConnectionMemoryRepository
    {
        private readonly ConcurrentDictionary<string, string> _map = new();

        public void Register(string hostname, string connectionId)
            => _map[hostname] = connectionId;

        public string GetConnectionId(string hostname)
         => _map.TryGetValue(hostname, out var connectionId) ? connectionId :
                throw new GenericErrorException(["NÃO ENCONTRADO CONEXÃO ABERTA PARA ESSE HOSTNAME"]);

        public void Remove(string hostname)
            => _map.TryRemove(hostname, out _);
    }
}
