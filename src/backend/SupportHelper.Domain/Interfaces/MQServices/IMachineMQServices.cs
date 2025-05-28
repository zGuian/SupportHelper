using SupportHelper.Domain.Entities;

namespace SupportHelper.Domain.Interfaces.MQServices
{
    public interface IMachineMQServices
    {
        Task<Machine?> GetInformationOnlyMachineAsync(string exchange, string routingKey, string message, Dictionary<string, object?>? headers = null, CancellationToken cancellationToken = default);
    }
}
