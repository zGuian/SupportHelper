using Microsoft.AspNetCore.SignalR.Client;

namespace SupportHelper.WinServices.Application.Interfaces.Events
{
    public interface IMachineEventHandler
    {
        void Register(HubConnection connection, CancellationToken cancellationToken);
    }
}
