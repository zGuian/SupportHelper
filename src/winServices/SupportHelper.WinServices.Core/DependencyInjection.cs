using SupportHelper.WinServices.Application.Interfaces.Events;
using SupportHelper.WinServices.Application.Interfaces.RabbitMQService;
using SupportHelper.WinServices.Application.Interfaces.Services;
using SupportHelper.WinServices.Application.Interfaces.UseCases;
using SupportHelper.WinServices.Application.Services;
using SupportHelper.WinServices.Application.Services.RabbitMQServices;
using SupportHelper.WinServices.Application.UseCases;
using SupportHelper.WinServices.Core.Events;
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
            services.AddTransient<IUpdateSgpClientUseCase, UpdateSgpClientUseCase>();
            return services;
        }
    }
}
