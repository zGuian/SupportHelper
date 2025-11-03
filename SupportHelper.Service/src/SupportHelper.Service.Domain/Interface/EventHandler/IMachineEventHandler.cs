using Microsoft.AspNetCore.SignalR.Client;
using SupportHelper.Service.Domain.Interface.Workers;

namespace SupportHelper.Service.Domain.Interface.EventHandler
{
    public interface IMachineEventHandler
    {
        void On(ISignalRWorker worker, HubConnection hubConnection);
    }
}