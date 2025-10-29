using SupportHelper.API.Domain.DTOs.Client;
using SupportHelper.API.Domain.DTOs.Requests;
using SupportHelper.API.Domain.DTOs.Responses;

namespace SupportHelper.API.Domain.Interfaces.Services
{
    public interface IMachineSignalRServices
    {
        Task<string> GetLogSgpClientAsync(string connectionId, RequestLogsSgpClientJson request, CancellationToken cancellationToken = default);
        Task<InfoMachineClient> RequestStatusAsync(string connectionId, CancellationToken cancellationToken = default);
        Task<IEnumerable<ResponseBase<ResponseUpdateSgpClientJson>>> UpdateManySgpClientAsync(IQueueUpdateSgpClient queueUpdateSgpClient, CancellationToken cancellationToken = default);
        Task<ResponseUpdateSgpClientJson> UpdateSgpClientAsync(string connectionId, RequestUpdateSgpClientJson requestJson, CancellationToken cancellationToken = default);
    }
}
