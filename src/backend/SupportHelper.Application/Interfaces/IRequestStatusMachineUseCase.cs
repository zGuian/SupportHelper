
using SupportHelper.Communication.Responses;

namespace SupportHelper.Application.Interfaces
{
    public interface IRequestStatusMachineUseCase
    {
        Task<ResponseStatusMachineJson> ExecuteAsync(string hostname, CancellationToken cancellationToken = default);
    }
}
