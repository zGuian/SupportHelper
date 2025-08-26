using SupportHelper.Communication.Requests;
using SupportHelper.Domain.Aggregates;
using SupportHelper.Domain.Interfaces.Models;

namespace SupportHelper.Domain.Interfaces.Repositories.Database
{
    public interface IMachineRepository
    {
        Task<IEnumerable<IMachineModel>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<IMachineModel> GetByHostnameAsync(string hostname, CancellationToken cancellationToken = default);
        Task<string> GetConnectionByHostnameAsync(string hostname, CancellationToken cancellationToken = default);
        Task<Dictionary<RequestUpdateSgpClientJson, string>> GetManyConnectionAsync(IEnumerable<RequestUpdateSgpClientJson> requests, CancellationToken ct = default);
        int GetQuantityMachines();
        Task InsertOrUpdateAsync(MachineAggregates aggregate, CancellationToken cancellationToken = default);
        Task InsertOrUpdateAsync(string hostname, string connId, CancellationToken cancellationToken = default);
    }
}
