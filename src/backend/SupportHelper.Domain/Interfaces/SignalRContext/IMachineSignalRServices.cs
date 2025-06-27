using SupportHelper.Communication.Responses;

namespace SupportHelper.Domain.Interfaces.SignalRContext
{
    public interface IMachineSignalRServices
    {
        Task<ResponseStatusMachineJson> RequestStatusAsync(string equipmentId, CancellationToken cancellationToken = default);
        Task<ResponseUpdateSgpClientJson> UpdateSgpClientAsync(string equipmentId, string productionLine, CancellationToken cancellationToken = default);
    }
}
