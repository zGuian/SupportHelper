using SupportHelper.Communication.Dtos.CouchDbDto;
using SupportHelper.Domain.Aggregates;
using SupportHelper.Domain.Entities;

namespace SupportHelper.Domain.Interfaces.Repositories.Database
{
    public interface IMachineRepository
    {
        Task<(AllDocsDto, int)> GetAllAsync(int limit, int skip, CancellationToken cancellationToken = default);
        Task<Machine> GetByHostnameAsync(string hostname, CancellationToken cancellationToken = default);
        Task<string> GetConnectionByHostnameAsync(string hostname, CancellationToken cancellationToken = default);
        Task InsertAsync(MachineSchemaJson schema, CancellationToken cancellationToken = default);
        Task InsertOrUpdateAsync(string hostname, string connId, CancellationToken cancellationToken = default);
        Task<ResponseBaseDto> InsertOrUpdateAsync(MachineSchemaJson schema, CancellationToken cancellationToken = default);
        Task UpdateAsync(MachineSchemaJson schema, CancellationToken cancellationToken = default);
        
    }
}
