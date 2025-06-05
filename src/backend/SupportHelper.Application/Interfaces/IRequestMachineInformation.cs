using SupportHelper.Communication.Requests;

namespace SupportHelper.Application.Interfaces
{
    public interface IRequestMachineInformation
    {
        Task ExecuteAsync(MachineInformationRequest request);
    }
}
