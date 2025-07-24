using SupportHelper.WinServices.Application.Interfaces.Events;
using SupportHelper.WinServices.Application.Interfaces.Services;
using SupportHelper.WinServices.Application.Interfaces.UseCases;
using SupportHelper.WinServices.Application.Services;
using SupportHelper.WinServices.Application.UseCases;
using SupportHelper.WinServices.Core.EventHandlers.MachineEvents;
using SupportHelper.WinServices.Core.EventHandlers.SignalREvents;
using SupportHelper.WinServices.Core.Workers;

namespace SupportHelper.WinServices.Core
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddDependencyInjection(this IServiceCollection services)
        {
            services.AddHostedService<SignalRWorker>();
            AddServices(services);
            AddUseCases(services);
            AddEventHandlers(services);
            return services;
        }

        private static void AddServices(IServiceCollection services)
        {
            services.AddSingleton<IMachineService, MachineService>();
        }

        private static void AddUseCases(IServiceCollection services)
        {
            services.AddSingleton<IGetLoggerSgpClientUseCase, GetLoggerSgpClientUseCase>();
            services.AddSingleton<IUpdateSgpClientUseCase, UpdateSgpClientUseCase>();
            services.AddSingleton<IFileTransferUseCase, FileTransferUseCase>();
        }

        private static void AddEventHandlers(IServiceCollection services)
        {
            services.AddSingleton<IWatchForShutdownHandler, WatchForShutdownHandler>();

            services.AddSingleton<ISignalREventHandler, ConnectionHandler>();
            services.AddSingleton<ISignalREventHandler, RequestLogSgpClientHandler>();
            services.AddSingleton<ISignalREventHandler, RequestStatusToMachineHandler>();
            services.AddSingleton<ISignalREventHandler, UpdateSgpClientHandler>();
        }
    }
}
