using Microsoft.Extensions.DependencyInjection;
using SupportHelper.RabbitMQ.Implementation;
using SupportHelper.RabbitMQ.Interfaces;

namespace SupportHelper.RabbitMQ
{
    public static class DependencyInjection
    {
        public static IServiceCollection CreateContainerHosted(this IServiceCollection services)
        {



            services.AddSingleton<RabbitMQConnection>();
            services.AddSingleton<IRabbitMQConnection>(sp => sp.GetRequiredService<RabbitMQConnection>());
            services.AddHostedService(sp => sp.GetRequiredService<RabbitMQConnection>());
            return services;
        }
    }
}
