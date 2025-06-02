using SupportHelper.Communication.Requests;
using SupportHelper.Domain.Entities;

namespace SupportHelper.Domain.Interfaces.MQServices
{
    public interface IMachineMQServices
    {
        Task<Guid> PublishByRouteKey(MachineInformationRequest request);
    }
}
