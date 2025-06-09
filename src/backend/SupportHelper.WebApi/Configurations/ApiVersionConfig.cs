using Asp.Versioning;

namespace SupportHelper.WebApi.Configurations
{
    public static class ApiVersionConfig
    {
        public static IServiceCollection AddConfigurationApiVersioning(this IServiceCollection services)
        {
            services.AddApiVersioning(opts =>
            {
                opts.DefaultApiVersion = new ApiVersion(1);
                opts.ReportApiVersions = true;
                opts.AssumeDefaultVersionWhenUnspecified = true;
            })
                .AddMvc()
                .AddApiExplorer(opts =>
                {
                    opts.GroupNameFormat = "'v'V";
                    opts.SubstituteApiVersionInUrl = true;
                });

            return services;
        }
    }
}
