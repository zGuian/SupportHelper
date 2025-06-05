using SupportHelper.Communication.Requests;

namespace SupportHelper.Domain.Interfaces.MQServices
{
    public interface IMachineMQServices
    {
        Task PublishGetInformationAsync(MachineInformationRequest request);
    }
}
