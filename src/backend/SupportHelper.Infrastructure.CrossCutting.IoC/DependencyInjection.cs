using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SupportHelper.Application.Interfaces;
using SupportHelper.Application.UseCases.MachineUC;
using SupportHelper.Domain.Interfaces.Repositories.Database;
using SupportHelper.Infrastructure.Data.Context;
using SupportHelper.Infrastructure.Data.Repositories.Database;
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
            return services;
        }

        private static void UseCasesDI(IServiceCollection services)
        {
            services.AddScoped<IRequestMachineInformationUseCase, RequestMachineInformationUseCase>();
            services.AddScoped<IRequestLogsSgpClientUseCase, RequestLogsSgpClientUseCase>();
        }

        private static void SignalRDI(this IServiceCollection services)
        {
            services.AddScoped<IConnectionService, ConnectionService>();
        }

        private static void DatabaseDI(IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(opts => opts.UseNpgsql(configuration.GetConnectionString("Default")));
            services.AddScoped<IMachineRepository, MachineRepository>();
        }
    }
}
