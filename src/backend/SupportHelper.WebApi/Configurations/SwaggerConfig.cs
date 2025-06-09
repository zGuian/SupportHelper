using Microsoft.OpenApi.Models;

namespace SupportHelper.WebApi.Configurations
{
    public static class SwaggerConfig
    {
        public static IServiceCollection AddSwaggerConfig(this IServiceCollection services)
        {
            services.AddSwaggerGen(opts =>
            {
                opts.SwaggerDoc("{v:apiVersion}", new OpenApiInfo
                {
                    Title = "SupportHelper",
                    Version = "v1"
                });
            });
            return services;
        }
    }
}
