using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SupportHelper.Application.Interfaces;
using SupportHelper.Application.UseCases.MachineUC;
using SupportHelper.Domain.Interfaces.MQServices;
using SupportHelper.Domain.Interfaces.Repositories.Database;
using SupportHelper.Domain.Interfaces.Repositories.Memory;
using SupportHelper.Domain.Interfaces.SignalRContext;
using SupportHelper.Exceptions.ExceptionsBase;
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
using System.Net.Http.Headers;

namespace SupportHelper.Infrastructure.CrossCutting.IoC
{
    public static class DependencyInjection
    {
        public static IServiceCollection IoC(this IServiceCollection services, IConfiguration configuration)
        {
            AddDatabase(services, configuration);
            AddSignalR(services);
            AddUseCases(services);
            AddHttpClient(services, configuration);
            return services;
        }

        private static void AddUseCases(IServiceCollection services)
        {
            services.AddScoped<IRequestGetAllMachinesUseCase, RequestGetAllMachinesUseCase>();
            services.AddScoped<IRequestLogsSgpClientUseCase, RequestLogsSgpClientUseCase>();
            services.AddScoped<IRequestMachineInformationUseCase, RequestMachineInformationUseCase>();
            services.AddScoped<IRequestStatusMachineUseCase, RequestStatusMachineUseCase>();
            services.AddScoped<IRequestUpdateSgpClientUseCase, RequestUpdateSgpClientUseCase>();
        }

        private static void AddSignalR(this IServiceCollection services)
        {
            services.AddSingleton<IConnectionService, ConnectionService>();
            services.AddSingleton<IMachineSignalRServices, MachineSignalRServices>();
        }

        private static void AddDatabase(IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(opts => opts.UseNpgsql(configuration.GetConnectionString("Default")));
            services.AddScoped<IMachineRepository, MachineRepository>();
            services.AddScoped<IMachineServices, MachineServices>();
            services.AddSingleton<IMachineMemoryRepository, MachineMemoryRepository>();
            services.AddSingleton<IConnectionMemoryRepository, ConnectionMemoryRepository>();
        }

        [Obsolete("RabbitMQ removido do projeto. Utilizar SignalR")]
        private static void RabbitMQDI(IServiceCollection services)
        {
            services.AddSingleton<IRabbitMQConnection, RabbitMQConnection>();
            services.AddSingleton<IMachineConsumer, MachineConsumer>();
            services.AddScoped<IMachineMQServices, MachineMQService>();
        }

        private static void AddHttpClient(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHttpClient("CouchDB", client =>
            {
                client.BaseAddress = new Uri(configuration.GetConnectionString("CouchDB")
                    ?? throw new GenericErrorException(["NÃO ENCONTRADO CONNECTION STRING"]));
                client.DefaultRequestHeaders.Add("Content-Type", "application/json");
            });
        }
    }
}
