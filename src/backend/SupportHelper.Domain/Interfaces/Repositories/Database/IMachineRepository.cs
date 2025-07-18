using SupportHelper.Communication.Dtos.CouchDbDto;
using SupportHelper.Domain.Aggregates;
using SupportHelper.Domain.Entities;

namespace SupportHelper.Domain.Interfaces.Repositories.Database
{
    public interface IMachineRepository
    {
        Task<AllDocsDto> GetAllAsync(int limit, int skip);
        Task<Machine> GetByHostname(string hostname);
        Task<string> GetConnectionByHostnameAsync(string hostname);
        Task InsertAsync(MachineSchemaJson schema);
        Task InsertOrUpdateAsync(string hostname, string connId);
        Task<ResponseBaseDto> InsertOrUpdateAsync(MachineSchemaJson schema);
        Task Login();
        Task UpdateAsync(MachineSchemaJson schema);
        
    }
}
