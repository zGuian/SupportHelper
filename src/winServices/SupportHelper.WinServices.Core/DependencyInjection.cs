using SupportHelper.RabbitMQ.Implementation;
using SupportHelper.RabbitMQ.Interfaces;
using SupportHelper.WinServices.Core.Interfaces;
using SupportHelper.WinServices.Core.Services;

namespace SupportHelper.WinServices.Core
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddDependencyInjection(this IServiceCollection services)
        {
            services.AddSingleton<RabbitMQConnection>();
            services.AddSingleton<IRabbitMQConnection>(sp => sp.GetRequiredService<RabbitMQConnection>());
            services.AddHostedService(sp => sp.GetRequiredService<RabbitMQConnection>());

            services.AddTransient<IMachineService, MachineService>();
            return services;
        }
    }
}
