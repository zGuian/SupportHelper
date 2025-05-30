using SupportHelper.Domain.Entities;

namespace SupportHelper.Domain.Interfaces.MQServices
{
    public interface IMachineMQServices_old
    {
        Task<Machine?> GetInformationOnlyMachineAsync(string exchange, string routingKey, string message, Dictionary<string, object?>? headers = null, CancellationToken cancellationToken = default);
    }
}
