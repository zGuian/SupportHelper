using SupportHelper.Communication.Dtos.CouchDbDto;
using SupportHelper.Domain.Aggregates;
using SupportHelper.Domain.Entities;

namespace SupportHelper.Domain.Interfaces.Repositories.Database
{
    public interface IMachineRepository
    {
        Task<AllDocsDto> GetAllAsync(int limit, int skip);
        Task<BaseDto> GetByIdAsync(string id);
        Task InsertAsync(MachineSchemaJson schema);
        Task<ResponseBaseDto> InsertOrUpdateAsync(MachineSchemaJson schema);
        Task UpdateAsync(MachineSchemaJson schema);
        
    }
}
