using SupportHelper.Communication.Requests;

namespace SupportHelper.Domain.Interfaces.MQServices
{
    public interface IMachineMQServices
    {
        //Task<Guid> PublishGetMachineInformation(MachineInformationRequest request);
        Task<Guid> GetInformationPublishAsync(MachineInformationRequest request);
    }
}
