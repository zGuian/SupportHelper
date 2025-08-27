using SupportHelper.Application.Interfaces;
using SupportHelper.Communication.Responses;
using SupportHelper.FrontEnd.MVC.Interfaces;
using SupportHelper.FrontEnd.MVC.Models;
using SupportHelper.FrontEnd.MVC.Models.ValueObjects;
using System.Text.Json;

namespace SupportHelper.FrontEnd.MVC.Services
{
    public class MachineServices : IMachineServices
    {
        private readonly ILogger<MachineServices> logger;
        private readonly IStatusMachineUseCase statusMachineUseCase;

        public MachineServices(ILogger<MachineServices> logger, IStatusMachineUseCase statusMachineUseCase)
        {
            this.statusMachineUseCase = statusMachineUseCase;
            this.logger = logger;
        }

        public async Task<MachineModel> GetMachineByHostnameAsync(string hostname, CancellationToken ct = default)
        {
            try
            {
                var response = await statusMachineUseCase.ExecuteAsync(hostname, ct);
                return new MachineModel(response.Id,
                                        response.Hostname,
                                        response.CurrentUsername,
                                        response.DomainName,
                                        response.OperationalSystem,
                                        response.NetworkBoards.Select(n => NetworkBoardVO.Create(n.Description, n.Ipv4, n.Ipv6, n.MacAddress, n.InUse)),
                                        response.UpTime,
                                        response.LastUpdate);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
