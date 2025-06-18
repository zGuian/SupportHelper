using Microsoft.AspNetCore.SignalR;
using SupportHelper.Domain.Interfaces.SignalRContext;
using SupportHelper.Infrastructure.SignalR.Hubs;
using SupportHelper.Infrastructure.SignalR.Interfaces;

namespace SupportHelper.Infrastructure.SignalR.SignalRServices
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
            string connId = GetConnectionId(equipmentId);
            await _context.Clients.Client(connId).SendAsync("RequestStatusToMachine");
        }

        public async Task UpdateSgpClientAsync(string equipmentId, string productionLine, CancellationToken cancellationToken = default)
        {
            string connId = GetConnectionId(equipmentId);
            await _context.Clients.Client(connId).SendAsync("UpdateSgpClient", productionLine, cancellationToken);
        }

        public async Task GetLogSgpClientAsync(string equipmentId, string productionLine, CancellationToken cancellationToken = default)
        {
            string connId = GetConnectionId(equipmentId);
            await _context.Clients.Client(connId).SendAsync("GetLogSgpClient", productionLine, cancellationToken);
        }

        private string GetConnectionId(string equipmentId)
        {
            return _connectionService.GetConnectionId(equipmentId) ?? throw new Exception("EQUIPAMENTO NÃO ESTA CONECTADO AO SIGNALR");
        }
    }
}
