using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SupportHelper.Application.Interfaces;
using SupportHelper.Application.UseCases.Requests;
using SupportHelper.Domain.Interfaces.Repositories.Database;
using SupportHelper.Domain.Interfaces.Repositories.Memory;
using SupportHelper.Domain.Interfaces.SignalRContext;
using SupportHelper.Exceptions.ExceptionsBase;
using SupportHelper.Infrastructure.Data.CouchDB.Repositories.Database;
using SupportHelper.Infrastructure.Data.CouchDB.Repositories.Memory;
using SupportHelper.Infrastructure.SignalR.Interfaces;
using SupportHelper.Infrastructure.SignalR.SignalRServices;
using SupportHelper.Infrastructure.SignalR.Tasks;
using System.Net.Http.Headers;
using System.Text;

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
            services.AddScoped<IGetAllMachinesUseCase, GetAllMachinesUseCase>();
            services.AddScoped<ILogsSgpClientUseCase, LogsSgpClientUseCase>();
            services.AddScoped<IMachineInformationUseCase, MachineInformationUseCase>();
            services.AddScoped<IStatusMachineUseCase, StatusMachineUseCase>();
            services.AddScoped<IUpdateSgpClientUseCase, UpdateSgpClientUseCase>();
        }

        private static void AddSignalR(this IServiceCollection services)
        {
            services.AddSingleton<IMachineSignalRServices, MachineSignalRServices>();
            services.AddSingleton<ITaskClientResponses, TaskClientResponses>();
        }

        private static void AddDatabase(IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<ITokenMemoryRepository, TokenMemoryRepository>();
            services.AddScoped<IMachineRepository, MachineRepository>();
        }

        private static void AddHttpClient(this IServiceCollection services, IConfiguration configuration)
        {
            var credentials = "admin:admin";
            var credentialsBytes = Encoding.UTF8.GetBytes(credentials);
            var base64Credentials = Convert.ToBase64String(credentialsBytes);

            services.AddHttpClient("CouchDB", client =>
            {
                client.BaseAddress = new Uri(configuration.GetConnectionString("CouchDB")
                    ?? throw new GenericErrorException(["NÃO ENCONTRADO CONNECTION STRING"]));
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", base64Credentials);
            });
        }
    }
}
