using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SupportHelper.Communication.Requests;

namespace SupportHelper.Infrastructure.MQServices
{
    public static class MassTransitServices
    {
        public static void MassTransitService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMassTransit(x =>
            {
                x.AddRequestClient<MachineInformationRequest>(TimeSpan.FromMinutes(1));

                x.UsingRabbitMq((context, cfg) =>
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
