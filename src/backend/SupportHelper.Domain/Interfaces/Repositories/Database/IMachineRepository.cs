using SupportHelper.Domain.Aggregates;

namespace SupportHelper.Domain.Interfaces.Repositories.Database
{
    public interface IMachineRepository
    {
        Task<IEnumerable<MachineAggregates>> GetAllAsync(int limit, int skip, CancellationToken cancellationToken = default);
        Task<MachineAggregates> GetByHostnameAsync(string hostname, CancellationToken cancellationToken = default);
        Task<string> GetConnectionByHostnameAsync(string hostname, CancellationToken cancellationToken = default);
        int GetQuantityMachines();
        Task InsertOrUpdateAsync(MachineAggregates aggregate, CancellationToken cancellationToken = default);
        Task InsertOrUpdateAsync(string hostname, string connId, CancellationToken cancellationToken = default);
    }
}
