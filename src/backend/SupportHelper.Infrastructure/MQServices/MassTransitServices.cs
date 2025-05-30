using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace SupportHelper.Infrastructure.MQServices
{
    public static class MassTransitServices
    {
        public static void MassTransitService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMassTransit(busConfigurator =>
            {
                busConfigurator.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host(new Uri(configuration["RabbitMQ:Configuration:Hostname"]!), h =>
                    {
                        h.Username(configuration["RabbitMQ:Configuration:Username"]!);
                        h.Password(configuration["RabbitMQ:Configuration:Password"]!);
                    });

                    cfg.ConfigureEndpoints(context);
                });
            });
        }
    }
}
