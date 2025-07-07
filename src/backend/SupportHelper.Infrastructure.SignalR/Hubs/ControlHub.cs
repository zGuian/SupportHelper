using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using SupportHelper.Communication.Responses;
using SupportHelper.Infrastructure.Data.Interfaces;
using SupportHelper.Infrastructure.SignalR.Interfaces;

namespace SupportHelper.Infrastructure.SignalR.Hubs
{
    public class ControlHub : Hub
    {
        private readonly IConnectionService _connectionService;
        private readonly IMachineServices _machineServices;

        public ControlHub(IConnectionService connectionService, IMachineServices machineServices)
        {
            _connectionService = connectionService;
            _machineServices = machineServices;
        }

        public async Task ClientHasShutdown(ResponseStatusMachineJson response)
        {
            await _machineServices.UpdateDatabaseAsync(response);
        }

        public async override Task OnConnectedAsync()
        {
            HttpContext httpContext = Context.GetHttpContext() ?? throw new Exception("NÃO ENCONTRADO VALORES DE URL");

            string hostName = httpContext.Request.Query["hostname"].ToString().ToLower();
            string connId = Context.ConnectionId;
            Console.WriteLine(hostName);
            _connectionService.Register(hostName, connId);
            await base.OnConnectedAsync();
        }
    }
}
