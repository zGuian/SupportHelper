using SupportHelper.API.Domain.DTOs.Entities;
using SupportHelper.API.Domain.DTOs.Generics;
using SupportHelper.API.Domain.DTOs.Requests;
using SupportHelper.API.Domain.DTOs.Responses;

namespace SupportHelper.API.Domain.Interfaces.Services
{
    public interface IMachineServices
    {
        Task<MachineDto> GetInformationAndUpdateDatabaseAsync(string hostname, CancellationToken ct = default);
        Task<MachineDto> GetInformationMachineInDatabaseAsync(string hostname, CancellationToken ct = default);
        Task<FileDataDto> GetLogsSgpClientAsync(RequestLogsSgpClientJson request, CancellationToken ct = default);
        Task RegisterConnectionAsync(string hostname, string connId, CancellationToken ct = default);
        Task<IEnumerable<ResponseBase<ResponseUpdateSgpClientJson>>> UpdateManySgpClientAsync(IEnumerable<RequestUpdateSgpClientJson> requests, CancellationToken ct = default);
        Task<ResponseUpdateSgpClientJson> UpdateOnlySgpClientAsync(RequestUpdateSgpClientJson requestJson, CancellationToken cancellationToken = default);
    }
}
