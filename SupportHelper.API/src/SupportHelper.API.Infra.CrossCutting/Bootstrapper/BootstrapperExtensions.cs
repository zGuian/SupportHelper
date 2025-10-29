using Mapster;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;
using SupportHelper.API.Domain.Interfaces.Repositories;
using SupportHelper.API.Domain.Interfaces.Repositories.Commons;
using SupportHelper.API.Domain.Interfaces.Services;
using SupportHelper.API.Domain.Services;
using SupportHelper.API.Domain.Utils.Converters;
using SupportHelper.API.Infra.Data.Context;
using SupportHelper.API.Infra.Data.Repositories.Database;
using SupportHelper.API.Infra.Data.Repositories.Database.Commons;
using SupportHelper.API.Infra.Data.Repositories.Memory;
using SupportHelper.API.Infra.SignalR.Interfaces;
using SupportHelper.API.Infra.SignalR.SignalRServices;
using SupportHelper.API.Infra.SignalR.Tasks;
using SupportHelper.API.Infra.SignalR.Utils;

namespace SupportHelper.API.Infra.CrossCutting.Bootstrapper
{
    public static class BootstrapperExtensions
    {
        public static IServiceCollection AddDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddStartUpConfig(configuration);
            services.AddInfraDependencies(configuration);
            services.AddServices();
            return services;
        }

        private static void AddStartUpConfig(this IServiceCollection services, IConfiguration configuration)
        {

        }

        private static void AddInfraDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext(configuration);
            services.AddRepositoriesDependencies(configuration);
            services.AddSignalRDependencies();
            services.AddMapsterDependecies();
        }

        private static void AddRepositoriesDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IMachineRepositoryQuery, MachineRepository>();
            services.AddScoped<IMachineRepositoryCommand, MachineRepository>();
            services.AddSingleton<ICacheTemp, CacheTemp>();

            services.AddSingleton<IConnectionMultiplexer>(sp =>
            {
                var config = configuration.GetConnectionString("Redis")
                    ?? throw new NotImplementedException("NÃO ENCOTNRADO VALORES DE CONEXÃO REDIS!");
                return ConnectionMultiplexer.Connect(config);
            });

            services.AddScoped<IUnitOfWork, UnitOfWork>();
        }

        private static void AddSignalRDependencies(this IServiceCollection services)
        {
            services.AddSignalR();
            services.AddScoped<IMachineSignalRServices, MachineSignalRServices>();
            services.AddSingleton<ITaskClientResponses, TaskClientResponses>();
            services.AddSingleton<IQueueProcess, QueueProcess>();
            services.AddSingleton<IQueueUpdateSgpClient, QueueUpdateSgpClientService>();
        }

        private static void AddDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(opts =>
                opts.UseSqlServer(configuration.GetConnectionString("SQLServer")
                    ?? throw new ArgumentNullException(nameof(configuration))));
        }

        private static void AddServices(this IServiceCollection services)
        {
            services.AddScoped<IMachineServices, MachineServices>();
            services.AddSingleton<IQueueUpdateSgpClient, QueueUpdateSgpClientService>();
        }

        private static void AddMapsterDependecies(this IServiceCollection services)
        {
            services.AddMapster();
            MapterConfig.Configure();
        }
    }
}
