using SupportHelper.FrontEnd.MVC.Interfaces;
using SupportHelper.FrontEnd.MVC.Services;
using System.Net.Http.Headers;
using System.Net.Mime;

namespace SupportHelper.FrontEnd.MVC
{
    public static class DependencyInjection
    {
        public static IServiceCollection IoC(this IServiceCollection services, IConfiguration configuration)
        {
            ServicesConfiguration(services);
            return services;
        }

        private static void ServicesConfiguration(IServiceCollection services)
        {
            services.AddScoped<IMachineServices, MachineServices>();

            services.AddHttpClient("Default", opts =>
            {
                opts.BaseAddress = new Uri("http://localhost:5001/api/v1/machine");
                opts.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(MediaTypeNames.Application.Json));
            });
        }
    }
}
