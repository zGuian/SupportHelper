using Microsoft.AspNetCore.SignalR;
using SupportHelper.Communication.Responses;
using SupportHelper.Domain.Interfaces.SignalRContext;
using SupportHelper.Infrastructure.SignalR.Hubs;
using SupportHelper.Infrastructure.SignalR.Interfaces;

namespace SupportHelper.Infrastructure.SignalR.SignalRServices
{
    public class MachineSignalRServices : IMachineSignalRServices
    {
        private readonly IHubContext<ControlHub> _context;
        private readonly ITaskClientResponses _taskClientResponse;

        public MachineSignalRServices(IHubContext<ControlHub> context, ITaskClientResponses taskClientResponse)
        {
            _context = context;
            _taskClientResponse = taskClientResponse;
        }

        public async Task<ResponseStatusMachineJson> RequestStatusAsync(string connectionId,
            CancellationToken cancellationToken = default)
        {
            var requestId = Guid.NewGuid().ToString();
            var tcs = new TaskCompletionSource<ResponseStatusMachineJson>();
            _taskClientResponse.Register(requestId, tcs);

            try
            {
                await _context.Clients.Clients(connectionId).SendAsync("RequestStatusMachine", requestId, cancellationToken);
                var response = await tcs.Task.WaitAsync(_taskClientResponse.Time);
                return response;
            }
            catch (TimeoutException)
            {
                throw new TimeoutException($"Aguardando resposta do cliente '{connectionId}' excedeu o tempo limite.");
            }
            catch (Exception)
            {
                throw new NotImplementedException();
            }
            finally
            {
                _taskClientResponse.Unregister(requestId);
            }
        }

        public async Task<ResponseUpdateSgpClientJson> UpdateSgpClientAsync(string connectionId, string productionLine,
            CancellationToken cancellationToken = default)
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
