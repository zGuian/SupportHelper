using RabbitMQ.Client;

namespace SupportHelper.WinServices.Core.Interfaces
{
    public interface IMachineService
    {
        Task GetInformationFromMachineAsync(IConfiguration configuration, CancellationToken cancellationToken);
    }
}
