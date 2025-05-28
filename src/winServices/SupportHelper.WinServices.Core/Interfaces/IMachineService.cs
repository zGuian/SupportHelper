using RabbitMQ.Client;

namespace SupportHelper.WinServices.Core.Interfaces
{
    public interface IMachineService
    {
        Task GetInformationFromMachineAsync(string exchange, string queueName, string routingKey = "",
            CancellationToken cancellationToken = default);
    }
}
