using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SupportHelper.Application.Interfaces;
using SupportHelper.Application.UseCases.MachineUC;
using SupportHelper.Domain.Interfaces.MQServices;
using SupportHelper.Domain.Interfaces.Repositories.Database;
using SupportHelper.Domain.Interfaces.Repositories.Memory;
using SupportHelper.Domain.Interfaces.SignalRContext;
using SupportHelper.Infrastructure.Contracts;
using SupportHelper.Infrastructure.Data.Context;
using SupportHelper.Infrastructure.Data.Interfaces;
using SupportHelper.Infrastructure.Data.Repositories.Database;
using SupportHelper.Infrastructure.Data.Repositories.Memory;
using SupportHelper.Infrastructure.Data.Services;
using SupportHelper.Infrastructure.MQServices;
using SupportHelper.Infrastructure.MQServices.MQResponses;
using SupportHelper.Infrastructure.SignalR.Interfaces;
using SupportHelper.Infrastructure.SignalR.SignalRServices;

namespace SupportHelper.Infrastructure.CrossCutting.IoC
{
    public static class DependencyInjection
    {
        public static IServiceCollection IoC(this IServiceCollection services, IConfiguration configuration)
        {
            DatabaseDI(services, configuration);
            SignalRDI(services);
            UseCasesDI(services);
            RabbitMQDI(services);
            return services;
        }

        private static void UseCasesDI(IServiceCollection services)
        {
            services.AddScoped<IRequestMachineInformationUseCase, RequestMachineInformationUseCase>();
            services.AddScoped<IRequestLogsSgpClientUseCase, RequestLogsSgpClientUseCase>();
            services.AddScoped<IRequestStatusMachineUseCase, RequestStatusMachineUseCase>();
        }

        private static void SignalRDI(this IServiceCollection services)
        {
            //services.AddSingleton<IConnectionService, ConnectionService>();
            services.AddSingleton<IMachineSignalRServices, MachineSignalRServices>();
        }

        private static void DatabaseDI(IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(opts => opts.UseNpgsql(configuration.GetConnectionString("Default")));
            services.AddScoped<IMachineRepository, MachineRepository>();
            services.AddScoped<IMachineServices, MachineServices>();
            services.AddSingleton<IMachineMemoryRepository, MachineMemoryRepository>();
            services.AddSingleton<IConnectionMemoryRepository, ConnectionMemoryRepository>();
        }

        private static void RabbitMQDI(IServiceCollection services)
        {
            services.AddSingleton<IRabbitMQConnection, RabbitMQConnection>();
            services.AddSingleton<IMachineConsumer, MachineConsumer>();
            services.AddScoped<IMachineMQServices, MachineMQService>();
        }
    }
}
