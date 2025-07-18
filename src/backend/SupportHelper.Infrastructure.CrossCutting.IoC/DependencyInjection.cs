using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SupportHelper.Application.Interfaces;
using SupportHelper.Application.UseCases.MachineUC;
using SupportHelper.Domain.Interfaces.Repositories.Database;
using SupportHelper.Domain.Interfaces.Repositories.Memory;
using SupportHelper.Domain.Interfaces.SignalRContext;
using SupportHelper.Exceptions.ExceptionsBase;
using SupportHelper.Infrastructure.Data.CouchDB.Repositories.Database;
using SupportHelper.Infrastructure.Data.CouchDB.Repositories.Memory;
using SupportHelper.Infrastructure.SignalR.SignalRServices;

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
            services.AddSingleton<IMachineSignalRServices, MachineSignalRServices>();
        }

        private static void AddDatabase(IServiceCollection services, IConfiguration configuration)
        {
            //services.AddSingleton<IConnectionMemoryRepository, ConnectionMemoryRepository>();
            services.AddSingleton<ITokenMemoryRepository, TokenMemoryRepository>();
            services.AddScoped<IMachineRepository, MachineRepository>();
        }

        private static void AddHttpClient(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHttpClient("CouchDB", client =>
            {
                client.BaseAddress = new Uri(configuration.GetConnectionString("CouchDB")
                    ?? throw new GenericErrorException(["NÃO ENCONTRADO CONNECTION STRING"]));
                client.DefaultRequestHeaders.Add("Accept", "application/json");
            });
        }
    }
}
