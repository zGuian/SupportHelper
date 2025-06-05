using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SupportHelper.Application.Interfaces;
using SupportHelper.Application.UseCases.MachineUC;

namespace SupportHelper.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationContext(this IServiceCollection services,
            IConfiguration configuration)
        {
            AddUseCases(services);
            return services;
        }

        private static void AddUseCases(IServiceCollection services)
        {
            services.AddScoped<IRequestMachineInformation, RequestMachineInformation>();
        }
    }
}
