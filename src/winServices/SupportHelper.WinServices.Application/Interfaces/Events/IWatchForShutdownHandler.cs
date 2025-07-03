using Microsoft.AspNetCore.SignalR.Client;

namespace SupportHelper.WinServices.Application.Interfaces.Events
{
    public interface IWatchForShutdownHandler
    {
        void Run(HubConnection connection, CancellationToken cancellationToken = default);
    }
}
