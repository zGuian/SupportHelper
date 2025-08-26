using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SupportHelper.Communication.Requests;
using SupportHelper.Communication.Responses;
using SupportHelper.Domain.Interfaces.ApplicationContext;
using SupportHelper.Domain.Interfaces.SignalRContext;
using SupportHelper.Infrastructure.SignalR.Hubs;
using SupportHelper.Infrastructure.SignalR.Interfaces;
using System.Collections.Immutable;
using System.Text.Json;

namespace SupportHelper.Infrastructure.SignalR.SignalRServices
{
    public class MachineSignalRServices : IMachineSignalRServices
    {
        private readonly IHubContext<ControlHub> _context;
        private readonly ITaskClientResponses _taskClientResponse;
        private readonly ILogger<MachineSignalRServices> _logger;
        private ImmutableList<Task<string>> _tasks = [];
        private List<ResponseUpdateSgpClientJson> _responsesJson = [];
        private List<ResponseBase<ResponseUpdateSgpClientJson>> _responseFinal = [];

        public MachineSignalRServices(IHubContext<ControlHub> context, ITaskClientResponses taskClientResponse,
            ILogger<MachineSignalRServices> logger)
        {
            _context = context;
            _taskClientResponse = taskClientResponse;
            _logger = logger;
        }

        public async Task<ResponseStatusMachineJson> RequestStatusAsync(string connectionId,
            CancellationToken cancellationToken = default)
        {
            var (requestId, tcs) = RegisterTcs();
            try
            {
                await _context.Clients.Clients(connectionId).SendAsync("RequestStatusMachine", requestId, cancellationToken);
                var response = await tcs.Task.WaitAsync(_taskClientResponse.Time, cancellationToken);
                return JsonConvert.DeserializeObject<ResponseStatusMachineJson>(response) ??
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

        public async Task<string> GetLogSgpClientAsync(string connectionId, RequestLogsSgpClientJson request,
            CancellationToken cancellationToken = default)
        {
            var (requestId, tcs) = RegisterTcs();
            try
            {
                await _context.Clients.Client(connectionId).SendAsync("GetLogSgpClient", request, requestId, cancellationToken);
                var response = await tcs.Task.WaitAsync(_taskClientResponse.Time, cancellationToken);
                return response;
            }
            catch (TimeoutException ex)
            {
                _logger.LogCritical("ERRO DE TIMEOUT {message}", ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogCritical("ERROR: {message}", ex.Message);
                throw;
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
                var response = await tcs.Task.WaitAsync(TimeSpan.FromMinutes(2), cancellationToken);
                return JsonConvert.DeserializeObject<ResponseUpdateSgpClientJson>(response) ??
                    throw new NotImplementedException();
            }
            catch (TimeoutException ex)
            {
                _logger.LogError("EXCEDEU LIMITE DE REQUISIÇÃO. RECEBIDO TIMEOUT {message}", ex.Message);
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

        public async Task<IEnumerable<ResponseBase<ResponseUpdateSgpClientJson>>> UpdateManySgpClientAsync(
            IQueueUpdateSgpClient queueUpdateSgpClient, CancellationToken cancellationToken = default)
        {
            while (queueUpdateSgpClient.QueueValues.Count > 0)
            {
                queueUpdateSgpClient.Dequeue(out var item);
                var (requestId, tcs) = RegisterTcs();
                try
                {
                    await _context.Clients.Clients(item.connId).SendAsync("UpdateSgpClient", requestId, item.request, cancellationToken);
                    _tasks.Add(tcs.Task.WaitAsync(TimeSpan.FromMinutes(2), cancellationToken));
                }
                catch (Exception ex)
                {
                    _logger.LogCritical("ERROR: {message}", ex.Message);
                    _responseFinal.Add(ResponseBase<ResponseUpdateSgpClientJson>.Factories.Error("Erro inesperado ao processar o item."));
                }
                finally
                {
                    _taskClientResponse.Unregister(requestId);
                }
            }

            await ProcessResponsesSignalR();
            _responsesJson.ForEach(r => _responseFinal.Add(ResponseBase<ResponseUpdateSgpClientJson>.Factories.Success(r)));
            return _responseFinal;
        }

        private async Task ProcessResponsesSignalR()
        {
            try
            {
                await Task.WhenAll(_tasks);
            }
            catch (Exception) { }

            foreach (var task in _tasks)
            {
                if (task.IsCompletedSuccessfully)
                {
                    var response = await task;
                    var objResponse = JsonConvert.DeserializeObject<ResponseUpdateSgpClientJson>(response) ?? throw new Exception();
                    _responsesJson.Add(objResponse);
                    continue;
                }
                var exception = task.Exception?.InnerException;
                if (exception is TimeoutException)
                {
                    _responseFinal.Add(ResponseBase<ResponseUpdateSgpClientJson>.Factories.Error("Timeout da resposta do cliente."));
                    continue;
                }
                _responseFinal.Add(ResponseBase<ResponseUpdateSgpClientJson>.Factories.Error($"Erro inesperado: {exception?.Message}"));
                _logger.LogError(exception, "Falha ao processar tarefa.");
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
