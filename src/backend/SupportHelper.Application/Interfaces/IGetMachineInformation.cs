using SupportHelper.Communication.Requests;
using SupportHelper.Communication.Responses;

namespace SupportHelper.Application.Interfaces
{
    public interface IGetMachineInformation
    {
        Task<MachineInformationResponse> ExecuteAsync(MachineInformationRequest request);
    }
}
