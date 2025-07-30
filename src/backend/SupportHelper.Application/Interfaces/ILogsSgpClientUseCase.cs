using SupportHelper.Communication.Requests;

namespace SupportHelper.Application.Interfaces
{
    public interface ILogsSgpClientUseCase
    {
        Task ExecuteAsync(RequestLogsSgpClientJson request, CancellationToken cancellationToken = default);
    }
}
