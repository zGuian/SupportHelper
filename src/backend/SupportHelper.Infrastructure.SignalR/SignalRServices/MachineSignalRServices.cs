using Microsoft.AspNetCore.SignalR;
using SupportHelper.Communication.Requests;
using SupportHelper.Communication.Responses;
using SupportHelper.Domain.Interfaces.SignalRContext;
using SupportHelper.Infrastructure.SignalR.Hubs;
using SupportHelper.Infrastructure.SignalR.Interfaces;
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
            var (requestId, tcs) = RegisterTcs();
            try
            {
                await _context.Clients.Clients(connectionId).SendAsync("RequestStatusMachine", requestId, cancellationToken);
                var response = await tcs.Task.WaitAsync(_taskClientResponse.Time, cancellationToken);
                return JsonSerializer.Deserialize<ResponseStatusMachineJson>(response) ??
                    throw new NotImplementedException();
            }
            catch (TimeoutException)
            {
                throw new TimeoutException($"Aguardando resposta do cliente '[{connectionId}]' excedeu o tempo limite.");
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

        public async Task<ResponseUpdateSgpClientJson> UpdateSgpClientAsync(string connectionId, RequestUpdateSgpClientJson requestJson,
            CancellationToken cancellationToken = default)
        {
            var (requestId, tcs) = RegisterTcs();
            try
            {
                await _context.Clients.Clients(connectionId).SendAsync("UpdateSgpClient", requestId, requestJson, cancellationToken);
                var response = await tcs.Task.WaitAsync(_taskClientResponse.Time, cancellationToken);
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
                _taskClientResponse.Unregister(requestId);
            }
        }

        public async Task<string> GetLogSgpClientAsync(RequestLogsSgpClientJson request, string connectionId, CancellationToken cancellationToken = default)
        {
            var (requestId, tcs) = RegisterTcs();
            try
            {
                await _context.Clients.Client(connectionId).SendAsync("GetLogSgpClient", request, requestId, cancellationToken);
                var response = await tcs.Task.WaitAsync(_taskClientResponse.Time, cancellationToken);
                return response;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private (string requestId, TaskCompletionSource<string> tcs) RegisterTcs()
        {
            var requestId = Guid.NewGuid().ToString();
            var tcs = new TaskCompletionSource<string>();
            _taskClientResponse.Register(requestId, tcs);
            return (requestId, tcs);
        }
    }
}
