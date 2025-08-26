using SupportHelper.Communication.Requests;
using SupportHelper.Communication.Responses;

namespace SupportHelper.Application.Interfaces
{
    public interface IUpdateManySgpClientUseCase
    {
        Task<IEnumerable<ResponseBase<ResponseUpdateSgpClientJson>>> ExecuteAsync(IEnumerable<RequestUpdateSgpClientJson> requests, CancellationToken ct = default);
    }
}
