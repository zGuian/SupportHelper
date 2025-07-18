using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using SupportHelper.Communication.Responses;
using SupportHelper.Domain.Aggregates;
using SupportHelper.Domain.Interfaces.Repositories.Database;
using SupportHelper.Domain.Interfaces.Repositories.Memory;
using SupportHelper.Exceptions.ExceptionsBase;

namespace SupportHelper.Infrastructure.SignalR.Hubs
{
    public class ControlHub : Hub
    {
        private readonly IMachineRepository _machineRepository;

        public ControlHub(IMachineRepository machineRepository)
        {
            _machineRepository = machineRepository;
        }

        public async Task ClientHasShutdown(ResponseStatusMachineJson response)
        {
            var connId = await _machineRepository.GetConnectionByHostnameAsync(response.Hostname);
            var schema = MachineSchemaJson.Create(response, connId);
            await _machineRepository.UpdateAsync(schema);
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
