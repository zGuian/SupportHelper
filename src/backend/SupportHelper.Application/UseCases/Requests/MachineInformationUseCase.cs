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
            var aggregate = await _repository.GetByHostnameAsync(hostname, cancellationToken);
            var responseMachine = new ResponseMachine
            {
                Id = aggregate.Machine.Id,
                IsConnected = aggregate.Machine.IsConnected,
                Hostname = aggregate.Machine.Hostname,
                SgpIsRunning = aggregate.Machine.SgpIsRunning,
                CurrentUsername = aggregate.Machine.CurrentUsername,
                DomainName = aggregate.Machine.DomainName,
                OperationalSystem = aggregate.Machine.OperationalSystem,
                UpTime = aggregate.Machine.UpTime,
                LastUpdate = aggregate.Machine.LastUpdate,
            };

            return responseMachine;
        }
    }
}