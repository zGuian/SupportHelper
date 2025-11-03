using SupportHelper.Service.CrossCutting.Bootstrapper;

namespace SupportHelper.Service.Application.HostedServices
{
    public class InitializeEvents(EventRegistrationBootstrapper events) : BackgroundService
    {
        private readonly EventRegistrationBootstrapper _events = events;

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await Task.Delay(TimeSpan.FromSeconds(30));
            _events.RegisterEvents(stoppingToken);
        }
    }
}
