using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SupportHelper.Domain.Interfaces.MQServices;
using SupportHelper.Domain.Interfaces.Repositories;
using SupportHelper.Infrastructure.MQServices;
using SupportHelper.Infrastructure.MQServices.Responses;
using SupportHelper.Infrastructure.Persistence.Context;
using SupportHelper.Infrastructure.Persistence.Repositories;

namespace SupportHelper.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureContext(this IServiceCollection services, IConfiguration configuration)
        {
            AddMqServices(services, configuration);
            AddDatabaseContext(services, configuration);
            return services;
        }

        private static void AddMqServices(IServiceCollection services, IConfiguration configuration)
        {
            services.MassTransitService(configuration);
            services.AddScoped<MachineResponseConsumer>();
            services.AddScoped<IMachineMQServices, MachineMQService>();
        }

        private static void AddDatabaseContext(IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(opts => opts.UseNpgsql(configuration.GetConnectionString("Default")));
            services.AddScoped<IMachineRepository, MachineRepository>();
        }
    }
}
