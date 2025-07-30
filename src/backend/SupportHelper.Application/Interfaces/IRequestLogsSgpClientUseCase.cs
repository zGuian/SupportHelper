using SupportHelper.Communication.Requests;

namespace SupportHelper.Application.Interfaces
{
    public interface IRequestLogsSgpClientUseCase
    {
        Task ExecuteAsync(RequestLogsSgpClientJson request, CancellationToken cancellationToken = default);
    }
}
