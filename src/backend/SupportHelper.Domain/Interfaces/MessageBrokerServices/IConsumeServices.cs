using SupportHelper.Domain.Entities;

namespace SupportHelper.Domain.Interfaces.MessageBrokerServices
{
    public interface IConsumeServices
    {
        string ConsumeMessage();
        Task<Machine> ConsumeMessageAsync(string queueName, bool autoAck);
    }
}
