using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using SupportHelper.Communication.Responses;
using SupportHelper.Domain.Interfaces.Repositories.Database;
using SupportHelper.Domain.Interfaces.Repositories.Memory;
using SupportHelper.Exceptions.ExceptionsBase;
using SupportHelper.Infrastructure.Data.Interfaces;
using SupportHelper.Infrastructure.Data.Repositories.Memory;
using SupportHelper.Infrastructure.SignalR.Interfaces;

namespace SupportHelper.Infrastructure.SignalR.Hubs
{
    public class ControlHub : Hub
    {
        private readonly IMachineServices _machineServices;
        private readonly IMachineRepository _machineRepository;
        private readonly IConnectionMemoryRepository _connectionMemoryRepository;

        public ControlHub(IMachineServices machineServices, IConnectionMemoryRepository connectionMemoryRepository,
            IMachineRepository machineRepository)
        {
            _machineServices = machineServices;
            _connectionMemoryRepository = connectionMemoryRepository;
            _machineRepository = machineRepository;
        }

        public async Task ClientHasShutdown(ResponseStatusMachineJson response)
        {
            await _machineServices.UpdateDatabaseAsync(response);
        }

        public async override Task OnConnectedAsync()
        {
            HttpContext httpContext = Context.GetHttpContext() ?? throw new GenericErrorException(["NÃO ENCONTRADO VALORES DE URL"]);

            string hostName = httpContext.Request.Query["hostname"].ToString().ToLower();
            string connId = Context.ConnectionId;
            _connectionMemoryRepository.Register(hostName, connId);
            await base.OnConnectedAsync();
        }
    }
}
