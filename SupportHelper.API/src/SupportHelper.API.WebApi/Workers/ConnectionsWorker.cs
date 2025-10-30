using SupportHelper.API.Domain.Interfaces.Repositories;
using SupportHelper.API.Domain.Interfaces.Services;
using SupportHelper.API.Infra.SignalR.Interfaces;

namespace SupportHelper.API.WebApi.Workers
{
    public class ConnectionsWorker(IQueueProcess queue
        , IServiceScopeFactory scopeFactory) : BackgroundService
    {
        private readonly IQueueProcess _queue = queue;
        private readonly IServiceScopeFactory _scopeFactory = scopeFactory;

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var result = await _queue.DequeueConnectionsAsync();

                using var scope = _scopeFactory.CreateScope();
                var machineServices = scope.ServiceProvider.GetRequiredService<IMachineServices>();
                var query = scope.ServiceProvider.GetRequiredService<IMachineRepositoryQuery>();

                if (!result.HasValue)
                {
                    continue;
                }
                await machineServices.RegisterConnectionAsync(result.Value.hostname, result.Value.connId, stoppingToken);
            }
        }
    }
}
