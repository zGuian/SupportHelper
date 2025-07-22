using Microsoft.AspNetCore.SignalR;
using SupportHelper.Communication.Requests;
using SupportHelper.Communication.Responses;
using SupportHelper.Domain.Interfaces.SignalRContext;
using SupportHelper.Infrastructure.SignalR.Hubs;
using SupportHelper.Infrastructure.SignalR.Interfaces;
using System.Text;
using System.Text.Json;

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
            var tuple = RegisterTcs();
            try
            {
                await _context.Clients.Clients(connectionId).SendAsync("RequestStatusMachine", tuple.Item1, cancellationToken);
                var response = await tuple.Item2.Task.WaitAsync(_taskClientResponse.Time);
                return JsonSerializer.Deserialize<ResponseStatusMachineJson>(response) ??
                    throw new NotImplementedException();
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
                _taskClientResponse.Unregister(tuple.Item1);
            }
        }

        public async Task<ResponseUpdateSgpClientJson> UpdateSgpClientAsync(string connectionId, RequestUpdateSgpClientJson requestJson, 
            CancellationToken cancellationToken = default)
        {
            var tuple = RegisterTcs();
            try
            {
                await _context.Clients.Clients(connectionId).SendAsync("UpdateSgpClient", tuple.Item1, requestJson, cancellationToken);
                var response = await tuple.Item2.Task.WaitAsync(_taskClientResponse.Time);
                return JsonSerializer.Deserialize<ResponseUpdateSgpClientJson>(response) ?? 
                    throw new NotImplementedException();
            }
            catch (TimeoutException ex)
            {
                throw new TimeoutException($"Aguardando resposta do cliente '{connectionId}' excedeu o tempo limite.");
            }
            catch (Exception ex)
            {
                throw new NotImplementedException();
            }
            finally
            {
                _taskClientResponse.Unregister(tuple.Item1);
            }
        }

        public async Task GetLogSgpClientAsync(string connectionId, string productionLine, CancellationToken cancellationToken = default)
        {
            await _context.Clients.Client(connectionId).SendAsync("GetLogSgpClient", productionLine, cancellationToken);
        }

        private (string, TaskCompletionSource<string>) RegisterTcs()
        {
            var requestId = Guid.NewGuid().ToString();
            var tcs = new TaskCompletionSource<string>();
            _taskClientResponse.Register(requestId, tcs);
            return (requestId, tcs);
        }
    }
}
