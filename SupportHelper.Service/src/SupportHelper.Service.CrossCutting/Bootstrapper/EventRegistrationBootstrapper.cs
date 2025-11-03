using Microsoft.AspNetCore.SignalR.Client;
using SupportHelper.Service.Domain.EventsHandlers;
using SupportHelper.Service.Domain.Interface.EventHandler;
using SupportHelper.Service.Domain.Interface.Workers;

namespace SupportHelper.Service.CrossCutting.Bootstrapper
{
    public class EventRegistrationBootstrapper(
        IEnumerable<ISignalREventHandler> signalrHandlers,
        MachineShutdownHandler shutdownHandler,
        ISignalRWorker signalRWorker,
        HubConnection connection)
    {
        private readonly IEnumerable<ISignalREventHandler> _signalrHandlers = signalrHandlers;
        private readonly MachineShutdownHandler _shutdownHandler = shutdownHandler;
        private readonly HubConnection _connection = connection;
        private readonly ISignalRWorker _signalRWorker = signalRWorker;

        public void RegisterEvents(CancellationToken stoppingToken)
        {
            RegisterSignalREvents(stoppingToken);
            RegisterMachineEvents();
        }

        private void RegisterSignalREvents(CancellationToken stoppingToken)
        {
            foreach (var handler in _signalrHandlers)
                handler.On(_connection, stoppingToken);
        }

        private void RegisterMachineEvents()
        {
            _shutdownHandler.On(_signalRWorker, _connection);
        }
    }
}
