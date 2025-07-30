using SupportHelper.Communication.Requests;
using SupportHelper.Communication.Responses;

namespace SupportHelper.Application.Interfaces
{
    public interface IUpdateSgpClientUseCase
    {
        Task<ResponseUpdateSgpClientJson> ExecuteAsync(RequestUpdateSgpClientJson requestJson);
    }
}
