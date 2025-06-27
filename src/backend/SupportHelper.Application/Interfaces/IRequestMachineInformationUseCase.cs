using SupportHelper.Communication.Requests;

namespace SupportHelper.Application.Interfaces
{
    public interface IRequestMachineInformationUseCase
    {
        Task ExecuteAsync(RequestMachineInformationJson request, CancellationToken cancellationToken = default);
    }
}
