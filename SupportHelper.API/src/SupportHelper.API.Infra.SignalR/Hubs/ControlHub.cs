using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using SupportHelper.API.Domain.Interfaces.Repositories;
using SupportHelper.API.Infra.SignalR.Interfaces;

namespace SupportHelper.API.Infra.SignalR.Hubs
{
    public class ControlHub(IMachineRepositoryCommand machineCommand
        , IQueueProcess queue) : Hub
    {
        private readonly IMachineRepositoryCommand _machineCommand = machineCommand;
        private readonly IQueueProcess _queue = queue;

        public async override Task OnConnectedAsync()
        {
            var httpContext = Context.GetHttpContext()
                ?? throw new Exception("NÃO ENCONTRADO VALORES DE URL");
            var hostname = httpContext.Request.Query["Hostname"];
            _queue.EnqueueConnections(Context.ConnectionId, hostname);
            await base.OnConnectedAsync();
        }

        public async Task ClientHasShutdown(string hostname)
        {
            await _machineCommand.UpdateForShutdownAsync(hostname);
        }

        public void ResponseBase(string requestId, string response) =>
            _queue.EnqueueResponses(requestId, response);
    }
}
