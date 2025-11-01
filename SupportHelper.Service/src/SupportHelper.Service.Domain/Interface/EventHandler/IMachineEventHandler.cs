using Microsoft.AspNetCore.SignalR.Client;
using SupportHelper.Service.Domain.Events;

namespace SupportHelper.Service.Domain.Interface.EventHandler
{
    public interface IMachineEventHandler
    {
        void Register(MachineEvents machineEvents, HubConnection hubConnection);
    }
}