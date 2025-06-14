using Microsoft.AspNetCore.SignalR;
using SupportHelper.Domain.Interfaces.SignalRContext;
using SupportHelper.Infrastructure.SignalR.Hubs;
using SupportHelper.Infrastructure.SignalR.Interfaces;

namespace SupportHelper.Infrastructure.SignalRContext
{
    public class MachineSignalRServices : IMachineSignalRServices
    {
        private readonly IHubContext<ControlHub> _context;
        private readonly IConnectionService _connectionService;

        public MachineSignalRServices(IHubContext<ControlHub> context, IConnectionService connectionService)
        {
            _context = context;
            _connectionService = connectionService;
        }

        public async Task RequestStatusAsync(string equipmentId)
        {
            string connId = _connectionService.GetConnectionId(equipmentId) ?? throw new Exception("EQUIPAMENTO NÃO ESTA CONECTADO AO SIGNALR");
            await _context.Clients.Client(connId).SendAsync("RequestStatusToMachine");
        }
    }
}
