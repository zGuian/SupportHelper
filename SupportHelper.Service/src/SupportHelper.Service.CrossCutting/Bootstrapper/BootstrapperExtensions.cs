using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SupportHelper.Service.Domain.EventsHandlers;
using SupportHelper.Service.Domain.Interface.EventHandler;
using SupportHelper.Service.Domain.Interface.Services;
using SupportHelper.Service.Domain.Interface.UseCases;
using SupportHelper.Service.Domain.Interface.Workers;
using SupportHelper.Service.Domain.Services;
using SupportHelper.Service.Domain.UseCases;

namespace SupportHelper.Service.CrossCutting.Bootstrapper
{
    public static class BootstrapperExtensions
    {
        public static IServiceCollection AddDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            AddEventHandlers(services);
            AddHttpContext(services);
            AddServices(services);
            AddUseCases(services);
            AddSignalRContext(services, configuration);

            services.AddSingleton<ISignalRWorker>(sp => (ISignalRWorker)sp.GetRequiredService<IHostedService>());
            return services;
        }

        private static void AddEventHandlers(IServiceCollection services)
        {
            services.AddSingleton<ISignalREventHandler, ConnectionHandler>();
            services.AddSingleton<ISignalREventHandler, RequestLogSgpClientHandler>();
            services.AddSingleton<ISignalREventHandler, RequestStatusToMachineHandler>();
            services.AddSingleton<ISignalREventHandler, UpdateSgpClientHandler>();

            services.AddSingleton<MachineShutdownHandler>();

            services.AddSingleton<EventRegistrationBootstrapper>();
        }

        private static void AddServices(IServiceCollection services)
        {
            services.AddTransient<IMachineServices, MachineService>();
        }

        private static void AddUseCases(IServiceCollection services)
        {
            services.AddTransient<IGetLoggerSgpClientUseCase, GetLoggerSgpClientUseCase>();
            services.AddTransient<IGetStatusMachineUseCase, GetStatusMachineUseCase>();
            services.AddTransient<IUpdateSgpClientUseCase, UpdateSgpClientUseCase>();
            services.AddSingleton<IFileTransferUseCase, FileTransferUseCase>();
        }

        private static void AddHttpContext(IServiceCollection services)
        {
            services.AddHttpClient("APISupportHelper", config =>
            {
                config.BaseAddress = new Uri("");
                config.Timeout = TimeSpan.FromSeconds(30);
            });
        }

        private static void AddSignalRContext(IServiceCollection services, IConfiguration configuration)
        {
            var url = configuration["SignalrSettings:Url"];
            if (string.IsNullOrEmpty(url))
            {
                throw new ArgumentNullException(nameof(url));
            }

            services.AddSingleton<HubConnection>(_ =>
            new HubConnectionBuilder().WithUrl($"{url}{Environment.MachineName}", opts =>
            {
                opts.Headers.Add("X-Hostname", Environment.MachineName);
                opts.Headers.Add("X-CurrentUsername", Environment.UserName);
                opts.Headers.Add("X-Uptime", TimeSpan.FromMilliseconds(Environment.TickCount64).ToString());
            })
            .WithAutomaticReconnect()
            .WithStatefulReconnect()
            .Build());
        }
    }
}
