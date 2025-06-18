using Microsoft.AspNetCore.SignalR.Client;

namespace SupportHelper.WinServices.Application.Interfaces.Events
{
    public interface ISignalREventHandler
    {
        void Register(HubConnection connection, CancellationToken stoppingToken);
    }
}
