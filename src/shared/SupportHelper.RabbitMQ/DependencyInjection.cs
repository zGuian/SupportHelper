using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SupportHelper.RabbitMQ.Implementation;
using SupportHelper.RabbitMQ.Interfaces;

namespace SupportHelper.RabbitMQ
{
    public static class DependencyInjection
    {
        public static IServiceCollection RabbitMQAbstract(this IServiceCollection services)
        {
            services.AddSingleton<RabbitMQConnection>();
            services.AddSingleton<IRabbitMQConnection>(sp => sp.GetRequiredService<RabbitMQConnection>());
            services.AddSingleton(typeof(IRabbitMQRequestReply<,>), typeof(RabbitMQRequestReply<,>));
            return services;
        }
    }
}
