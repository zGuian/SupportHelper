using SupportHelper.Communication.Requests;

namespace SupportHelper.Application.Interfaces
{
    public interface IGetMachineInformation
    {
        Task ExecuteAsync(MachineInformationRequest request);
    }
}
