using Microsoft.AspNetCore.SignalR.Client;
using SupportHelper.Service.Domain.Events;
using SupportHelper.Service.Domain.Interface.EventHandler;

namespace SupportHelper.Service.Application.HostedServices
{
    public class EventRegistration(IEnumerable<ISignalREventHandler> signalrHandlers, IEnumerable<IMachineEventHandler> machineHandlers,
        MachineEvents machineEvents, HubConnection connection)
    {
        private readonly IEnumerable<ISignalREventHandler> _signalrHandlers = signalrHandlers;
        private readonly IEnumerable<IMachineEventHandler> _machineHandlers = machineHandlers;
        private readonly MachineEvents _machineEvents = machineEvents;
        private readonly HubConnection _connection = connection;

        public Task RegisterEvents(CancellationToken stoppingToken)
        {
            foreach (ISignalREventHandler handler in _signalrHandlers)
            {
                handler.Register(_connection, stoppingToken);
            }

            foreach (var handler in _machineHandlers)
                handler.Register(_machineEvents, _connection);

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    }
}
