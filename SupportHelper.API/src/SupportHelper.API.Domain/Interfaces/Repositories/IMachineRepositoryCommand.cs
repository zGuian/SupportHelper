using SupportHelper.API.Domain.Entities;
using SupportHelper.API.Domain.Interfaces.Repositories.Commons;

namespace SupportHelper.API.Domain.Interfaces.Repositories
{
    public interface IMachineRepositoryCommand : IBaseRepositoryCommand<Machine, string>
    {
        Task UpdateAsync(Machine entity);
        Task UpdateForShutdownAsync(string hostname);
    }
}
