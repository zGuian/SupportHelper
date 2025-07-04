using SupportHelper.WinServices.Application.Interfaces.Events;
using SupportHelper.WinServices.Application.Interfaces.RabbitMQService;
using SupportHelper.WinServices.Application.Interfaces.Services;
using SupportHelper.WinServices.Application.Interfaces.UseCases;
using SupportHelper.WinServices.Application.Services;
using SupportHelper.WinServices.Application.Services.RabbitMQServices;
using SupportHelper.WinServices.Application.UseCases;
using SupportHelper.WinServices.Core.EventHandlers.MachineEvents;
using SupportHelper.WinServices.Core.EventHandlers.RabbitMQEvents;
using SupportHelper.WinServices.Core.EventHandlers.SignalREvents;
using SupportHelper.WinServices.Core.Workers;

namespace SupportHelper.WinServices.Core
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddDependencyInjection(this IServiceCollection services)
        {
            AddRabbitMQ(services);
            services.AddHostedService<RabbitEventWorker>();
            services.AddHostedService<SignalRWorker>();
            AddServices(services);
            AddUseCases(services);
            AddEventHandlers(services);
            return services;
        }

        private static void AddServices(IServiceCollection services)
        {
            services.AddTransient<IMachineService, MachineService>();
        }

        private static void AddUseCases(IServiceCollection services)
        {
            services.AddTransient<IGetLoggerSgpClientUseCase, GetLoggerSgpClientUseCase>();
            services.AddTransient<IUpdateSgpClientUseCase, UpdateSgpClientUseCase>();
        }

        private static void AddEventHandlers(IServiceCollection services)
        {
            services.AddTransient<IWatchForShutdownHandler, WatchForShutdownHandler>();

            services.AddSingleton<ISignalREventHandler, ConnectionHandler>();

            services.AddTransient<ISignalREventHandler, GetLogSgpClientHandler>();
            services.AddTransient<ISignalREventHandler, RequestStatusToMachineHandler>();
            services.AddTransient<ISignalREventHandler, UpdateSgpClientHandler>();
        }

        private static void AddRabbitMQ(IServiceCollection services)
        {
            services.AddSingleton<IRabbitConnectionService, RabbitConnectionService>();
            services.AddSingleton<IRabbitMQEvent, RabbitMQEvent>();
        }
    }
}
