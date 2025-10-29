using Microsoft.AspNetCore.SignalR.Client;

namespace SupportHelper.Service.Domain.Interface.EventHandler
{
    public interface ISignalREventHandler
    {
        void Register(HubConnection connection, CancellationToken stoppingToken);
    }
}
