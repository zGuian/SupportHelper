using Microsoft.AspNetCore.SignalR.Client;

namespace SupportHelper.Service.Domain.Interface.EventHandler
{
    public interface ISignalREventHandler
    {
        void On(HubConnection connection, CancellationToken stoppingToken);
    }
}
