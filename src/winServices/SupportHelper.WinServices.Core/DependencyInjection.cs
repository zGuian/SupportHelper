using SupportHelper.WinServices.Core.Events;
using SupportHelper.WinServices.Core.Interfaces;
using SupportHelper.WinServices.Core.Interfaces.Events;
using SupportHelper.WinServices.Core.Interfaces.RabbitMQService;
using SupportHelper.WinServices.Core.Interfaces.UseCases;
using SupportHelper.WinServices.Core.Services;
using SupportHelper.WinServices.Core.Services.RabbitMQServices;
using SupportHelper.WinServices.Core.UseCases;
using SupportHelper.WinServices.Core.Workers;

namespace SupportHelper.WinServices.Core
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddDependencyInjection(this IServiceCollection services)
        {
            services.AddSingleton<IRabbitConnectionService, RabbitConnectionService>();
            services.AddSingleton<IRabbitMQEvent, RabbitMQEvent>();

            services.AddHostedService<RabbitEventWorker>();

            services.AddTransient<IMachineService, MachineService>();
            services.AddTransient<IGetLoggerSgpClientUseCase, GetLoggerSgpClientUseCase>();
            return services;
        }
    }
}
