using SupportHelper.API.Domain.DTOs.Requests;
using SupportHelper.API.Domain.Entities;
using SupportHelper.API.Domain.Interfaces.Repositories.Commons;

namespace SupportHelper.API.Domain.Interfaces.Repositories
{
    public interface IMachineRepositoryQuery : IBaseRepositoryQuery<Machine, string>
    {
        Task<bool> ExistHostname(string hostname);
        Task<Machine> GetByHostnameAsync(string hostname, CancellationToken cancellationToken = default);
        Task<string> GetConnectionByHostnameAsync(string hostname, CancellationToken cancellationToken = default);
        Task<Dictionary<RequestUpdateSgpClientJson, string>> GetManyConnectionAsync(IEnumerable<RequestUpdateSgpClientJson> requests, CancellationToken ct = default);
        Task<(IEnumerable<Machine> models, int count)> GetPageAsync(int page, int pageSize);
    }
}
