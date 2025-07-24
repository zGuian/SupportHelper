using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using SupportHelper.Communication.Responses;
using SupportHelper.Domain.Aggregates;
using SupportHelper.Domain.Interfaces.Repositories.Database;
using SupportHelper.Exceptions.ExceptionsBase;
using SupportHelper.Infrastructure.SignalR.Interfaces;

namespace SupportHelper.Infrastructure.SignalR.Hubs
{
    public class ControlHub : Hub
    {
        private readonly IMachineRepository _machineRepository;
        private readonly ITaskClientResponses _taskClientResponses;

        public ControlHub(IMachineRepository machineRepository, ITaskClientResponses taskClientResponses)
        {
            _machineRepository = machineRepository;
            _taskClientResponses = taskClientResponses;
        }

        public async Task ClientHasShutdown(ResponseStatusMachineJson response)
        {
            var connId = await _machineRepository.GetConnectionByHostnameAsync(response.Hostname);
            var schema = MachineSchemaJson.Create(response, connId);
            await _machineRepository.UpdateAsync(schema);
        }

        public async Task ResponseStatusAsync(string requestId, string response)
        {
            _taskClientResponses.FinalizeTask(requestId, response);
            await Task.CompletedTask;
        }

        public async Task ResponseUpdateSgpClient(string requestId, string response)
        {
            _taskClientResponses.FinalizeTask(requestId, response);
            await Task.CompletedTask;
        }

        public void ResponseGetLogsSgpClient(string requestId, string response)
        {
            _taskClientResponses.FinalizeTask(requestId, response);
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
    }
}
