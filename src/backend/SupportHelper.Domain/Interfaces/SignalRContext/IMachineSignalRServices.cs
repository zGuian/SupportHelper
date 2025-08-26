using SupportHelper.Communication.Requests;
using SupportHelper.Communication.Responses;
using SupportHelper.Domain.Interfaces.ApplicationContext;

namespace SupportHelper.Domain.Interfaces.SignalRContext
{
    public interface IMachineSignalRServices
    {
        Task<string> GetLogSgpClientAsync(string connectionId, RequestLogsSgpClientJson request,
            CancellationToken cancellationToken = default);
        Task<ResponseStatusMachineJson> RequestStatusAsync(string equipmentId, 
            CancellationToken cancellationToken = default);
        Task<ResponseUpdateSgpClientJson> UpdateSgpClientAsync(string connectionId, 
            RequestUpdateSgpClientJson requestJson, CancellationToken cancellationToken = default);
        Task<IEnumerable<ResponseBase<ResponseUpdateSgpClientJson>>> UpdateManySgpClientAsync(IQueueUpdateSgpClient queueUpdateSgpClient, 
            CancellationToken cancellationToken = default);
    }
}
