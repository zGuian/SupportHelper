using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Logging;
using SupportHelper.Service.Domain.Events;
using SupportHelper.Service.Domain.Interface.EventHandler;
using SupportHelper.Service.Domain.Interface.UseCases;
using System.Text.Json;

namespace SupportHelper.Service.Domain.EventHandler
{
    public class ShutdownHandler(ILogger<ShutdownHandler> logger
        , IGetStatusMachineUseCase getStatusMachine) : IMachineEventHandler
    {
        private readonly ILogger<ShutdownHandler> _logger = logger;
        private readonly IGetStatusMachineUseCase _getStatusMachineUseCase = getStatusMachine;

        public void Register(MachineEvents machineEvents, HubConnection hubConnection)
        {
            machineEvents.OnShutdown += async () =>
            {
                var jsonString = await Task.Run(() =>
                {
                    var machine = _getStatusMachineUseCase.Execute();
                    return JsonSerializer.Serialize(machine);
                });

                await hubConnection.SendAsync("ClientHasShutdown", jsonString);
            };
        }
    }
}
