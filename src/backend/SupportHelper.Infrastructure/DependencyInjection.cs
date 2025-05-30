using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SupportHelper.Domain.Interfaces.MQServices;
using SupportHelper.Infrastructure.MQServices;
using SupportHelper.RabbitMQ.Implementation;
using SupportHelper.RabbitMQ.Interfaces;

namespace SupportHelper.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureContext(this IServiceCollection services, IConfiguration configuration)
        {
            AddMqServices(services, configuration);
            return services;
        }

        private static void AddMqServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<RabbitMQConnection>();
            services.AddSingleton<IRabbitMQConnection>(sp => sp.GetRequiredService<RabbitMQConnection>());
            services.AddHostedService(sp => sp.GetRequiredService<RabbitMQConnection>());
            services.AddSingleton<IRabbitMQProducer, RabbitMQProducer>();
            services.AddSingleton<IRabbitMQConsumer, RabbitMQConsumer>();

            services.AddScoped<IMachineMQServices, MachineMQServices>();
        }
    }
}
