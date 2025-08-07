using Microsoft.Extensions.Logging;
using SupportHelper.Application.Interfaces;
using SupportHelper.Communication.Responses;
using SupportHelper.Domain.Interfaces.Repositories.Database;

namespace SupportHelper.Application.UseCases.Requests
{
    public class MachineInformationUseCase : IMachineInformationUseCase
    {
        private readonly ILogger<MachineInformationUseCase> _logger;
        private readonly IMachineRepository _repository;

        public MachineInformationUseCase(ILogger<MachineInformationUseCase> logger, IMachineRepository repository)
        {
            _logger = logger;
            _repository = repository;
        }

        public async Task<ResponseMachine> ExecuteAsync(string hostname, CancellationToken cancellationToken = default)
        {
            var schema = await _repository.GetByHostnameAsync(hostname, cancellationToken);
            var responseMachine = new ResponseMachine
            {
                Id = schema.Machine.Id,
                IsConnected = schema.Machine.IsConnected,
                Hostname = schema.Machine.Hostname,
                SgpIsRunning = schema.Machine.SgpIsRunning,
                CurrentUsername = schema.Machine.CurrentUsername,
                DomainName = schema.Machine.DomainName,
                OperationalSystem = schema.Machine.OperationalSystem,
                NetworkBoards = schema.Machine.NetworkBoards.Select(nb => NetworkBoardResponse.Create(nb.Description, nb.Ipv4, nb.Ipv6, nb.MacAddress, nb.InUse)),
                UpTime = schema.Machine.UpTime,
                LastUpdate = schema.Machine.LastUpdate,
            };

            return responseMachine;
        }
    }
}