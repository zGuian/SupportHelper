using SupportHelper.Domain.Entities;

namespace SupportHelper.Domain.Interfaces.MQServices
{
    public interface IConsumeServices
    {
        string ConsumeMessage();
        Task<Machine> ConsumeMessageAsync(string queueName, bool autoAck);
    }
}
