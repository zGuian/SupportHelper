using SupportHelper.Communication.Requests;

namespace SupportHelper.Application.Interfaces
{
    public interface IRequestMachineInformationUseCase
    {
        Task ExecuteAsync(RequestStatusMachineJson request, CancellationToken cancellationToken = default);
    }
}
