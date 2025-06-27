using Microsoft.AspNetCore.SignalR;
using SupportHelper.Communication.Responses;
using SupportHelper.Domain.Interfaces.SignalRContext;
using SupportHelper.Infrastructure.SignalR.Hubs;

namespace SupportHelper.Infrastructure.SignalR.SignalRServices
{
    public class MachineSignalRServices : IMachineSignalRServices
    {
        private readonly IHubContext<ControlHub> _context;

        public MachineSignalRServices(IHubContext<ControlHub> context)
        {
            _context = context;
        }

        public async Task<ResponseStatusMachineJson> RequestStatusAsync(string connectionId, CancellationToken cancellationToken = default)
        {
            ResponseStatusMachineJson response = await _context.Clients.Client(connectionId)
                .InvokeAsync<ResponseStatusMachineJson>("RequestStatusMachine", cancellationToken);
            return response;
        }

        public async Task<ResponseUpdateSgpClientJson> UpdateSgpClientAsync(string connectionId, string productionLine, CancellationToken cancellationToken = default)
        {
            ResponseUpdateSgpClientJson response = await _context.Clients.Client(connectionId)
                .InvokeAsync<ResponseUpdateSgpClientJson>("UpdateSgpClient", productionLine, cancellationToken);
            return response;
        }

        public async Task GetLogSgpClientAsync(string connectionId, string productionLine, CancellationToken cancellationToken = default)
        {
            await _context.Clients.Client(connectionId).SendAsync("GetLogSgpClient", productionLine, cancellationToken);
        }
    }
}
