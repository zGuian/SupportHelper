using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using SupportHelper.Communication.Responses;
using SupportHelper.Domain.Aggregates;
using SupportHelper.Domain.Interfaces.Repositories.Database;
using SupportHelper.Exceptions.ExceptionsBase;
using SupportHelper.Infrastructure.SignalR.Interfaces;
using System.Collections.Concurrent;
using System.Text.Json;

namespace SupportHelper.Infrastructure.SignalR.Hubs
{
    public class ControlHub : Hub
    {
        private readonly IMachineRepository _machineRepository;
        private readonly ITaskClientResponses _taskClientResponses;
        private readonly IQueueProcess _queue;

        public ControlHub(IMachineRepository machineRepository, ITaskClientResponses taskClientResponses, 
            IQueueProcess queue)
        {
            _machineRepository = machineRepository;
            _taskClientResponses = taskClientResponses;
            _queue = queue;
        }

        public async override Task OnConnectedAsync()
        {
            HttpContext httpContext = Context.GetHttpContext()
            ?? throw new GenericErrorException(["NÃO ENCONTRADO VALORES DE URL"]);
            string hostname = httpContext.Request.Query["hostname"].ToString().ToLower();
            string connId = Context.ConnectionId;
            await _machineRepository.InsertOrUpdateAsync(hostname, connId);
            await base.OnConnectedAsync();
        }

        public async Task ClientHasShutdown(string response)
        {
            var obj = JsonSerializer.Deserialize<ResponseStatusMachineJson>(response);
            var connId = await _machineRepository.GetConnectionByHostnameAsync(obj.Hostname);
            var aggregate = MachineAggregates.Converters.ToAggregate(obj, connId, false);
            await _machineRepository.InsertOrUpdateAsync(aggregate);
        }

        public void ResponseStatusAsync(string requestId, string response) => 
            _queue.Enqueue(requestId, response);

        public void ResponseUpdateSgpClient(string requestId, string response) => 
            _queue.Enqueue(requestId, response);

        public void ResponseGetLogsSgpClient(string requestId, string response) =>
            _queue.Enqueue(requestId, response);
    }
}
