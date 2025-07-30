
using SupportHelper.Communication.Responses;

namespace SupportHelper.Application.Interfaces
{
    public interface IStatusMachineUseCase
    {
        Task<ResponseStatusMachineJson> ExecuteAsync(string hostname, CancellationToken cancellationToken = default);
    }
}
