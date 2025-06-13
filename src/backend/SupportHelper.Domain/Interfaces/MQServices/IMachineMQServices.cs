using SupportHelper.Communication.Requests;

namespace SupportHelper.Domain.Interfaces.MQServices
{
    public interface IMachineMQServices
    {
        Task PublishMessageAsync(RequestBase<RequestMachine> request, RabbitMQRequest rabbitMQRequest);
        Task PublishMessageAsync(MachineInformationRequest request);
    }
}
