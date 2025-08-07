using SupportHelper.Communication.Responses;

namespace SupportHelper.Application.Interfaces
{
    public interface IMachineInformationUseCase
    {
        Task<ResponseMachine> ExecuteAsync(string hostname, CancellationToken cancellationToken = default);
    }
}
