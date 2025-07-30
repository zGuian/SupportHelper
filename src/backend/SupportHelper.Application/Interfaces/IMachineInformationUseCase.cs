using SupportHelper.Communication.Requests;

namespace SupportHelper.Application.Interfaces
{
    public interface IMachineInformationUseCase
    {
        Task ExecuteAsync(RequestStatusMachineJson request, CancellationToken cancellationToken = default);
    }
}
