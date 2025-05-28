using RabbitMQ.Client;

namespace SupportHelper.WinServices.Core.Interfaces
{
    public interface IMachineService
    {
        Task GetInformationFromMachineAsync(string exchange, string routingKey, string queueName, 
            CancellationToken cancellationToken = default);
    }
}
