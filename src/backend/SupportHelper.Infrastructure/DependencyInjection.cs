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

        private static async void AddMqServices(IServiceCollection services, IConfiguration configuration)
        {
            var rabbitConnection = await RabbitMQConnection.CreateConnectionToRabbitMQ(configuration);
            services.AddSingleton<IRabbitMQConnection>(rabbitConnection);
            services.AddSingleton<IRabbitMQProducer, RabbitMQProducer>();
            services.AddSingleton<IRabbitMQConsumer, RabbitMQConsumer>();

            services.AddScoped<IMachineMQServices, MachineMQServices>();
        }
    }
}
