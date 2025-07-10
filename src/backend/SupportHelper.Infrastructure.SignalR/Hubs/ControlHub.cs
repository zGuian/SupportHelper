using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using SupportHelper.Communication.Responses;
using SupportHelper.Domain.Interfaces.Repositories.Memory;
using SupportHelper.Exceptions.ExceptionsBase;
using SupportHelper.Infrastructure.Data.Interfaces;
using SupportHelper.Infrastructure.Data.Repositories.Memory;
using SupportHelper.Infrastructure.SignalR.Interfaces;

namespace SupportHelper.Infrastructure.SignalR.Hubs
{
    public class ControlHub : Hub
    {
        private readonly IConnectionService _connectionService;
        private readonly IMachineServices _machineServices;
        private readonly IConnectionMemoryRepository _connectionMemoryRepository;

        public ControlHub(IConnectionService connectionService, IMachineServices machineServices,
            IConnectionMemoryRepository connectionMemoryRepository)
        {
            _connectionService = connectionService;
            _machineServices = machineServices;
            _connectionMemoryRepository = connectionMemoryRepository;
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
            Console.WriteLine(hostName);
            _connectionMemoryRepository.Register(hostName, connId);
            await base.OnConnectedAsync();
        }
    }
}
