using Microsoft.Extensions.DependencyInjection;
using SupportHelper.Service.Domain.EventHandler;
using SupportHelper.Service.Domain.Interface.EventHandler;
using SupportHelper.Service.Domain.Interface.Services;
using SupportHelper.Service.Domain.Interface.UseCases;
using SupportHelper.Service.Domain.Services;
using SupportHelper.Service.Domain.UseCases;

namespace SupportHelper.Service.CrossCutting.Bootstrapper
{
    public static class BootstrapperExtensions
    {
        public static IServiceCollection AddDependencies(this IServiceCollection services)
        {
            AddEventHandlers(services);
            AddHttpContext(services);
            AddServices(services);
            AddUseCases(services);
            return services;
        }

        private static void AddEventHandlers(IServiceCollection services)
        {
            services.AddSingleton<ISignalREventHandler, ConnectionHandler>();
            services.AddSingleton<ISignalREventHandler, RequestLogSgpClientHandler>();
            services.AddSingleton<ISignalREventHandler, RequestStatusToMachineHandler>();
            services.AddSingleton<ISignalREventHandler, UpdateSgpClientHandler>();
            services.AddSingleton<IFileTransferUseCase, FileTransferUseCase>();
        }

        private static void AddServices(IServiceCollection services)
        {
            services.AddTransient<IMachineServices, MachineService>();
        }

        private static void AddUseCases(IServiceCollection services)
        {
            services.AddTransient<IGetLoggerSgpClientUseCase,GetLoggerSgpClientUseCase>();
            services.AddTransient<IGetStatusMachineUseCase, GetStatusMachineUseCase>();
            services.AddTransient<IUpdateSgpClientUseCase, UpdateSgpClientUseCase>();
        }

        private static void AddHttpContext(IServiceCollection services)
        {
            services.AddHttpClient("APISupportHelper", config =>
            {
                config.BaseAddress = new Uri("");
                config.Timeout = TimeSpan.FromSeconds(30);
            });
        }
    }
}
