using SupportHelper.Application.Interfaces;
using SupportHelper.Communication.Responses;
using SupportHelper.Domain.Aggregates;
using SupportHelper.Domain.Interfaces.Repositories.Database;
using SupportHelper.Domain.Interfaces.SignalRContext;

namespace SupportHelper.Application.UseCases.Requests
{
    public class StatusMachineUseCase : IStatusMachineUseCase
    {
        private readonly IMachineSignalRServices _signalR;
        private readonly IMachineRepository _machineRepository;

        public StatusMachineUseCase(IMachineSignalRServices signalR, IMachineRepository machineRepository)
        {
            _signalR = signalR;
            _machineRepository = machineRepository;
        }

        public async Task<ResponseStatusMachineJson> ExecuteAsync(string hostname, CancellationToken cancellationToken = default)
        {
            var connId = await _machineRepository.GetConnectionByHostnameAsync(hostname, cancellationToken);
            var responseJson = await _signalR.RequestStatusAsync(connId, cancellationToken);
            var schema = MachineSchemaJson.Create(responseJson, connId);
            await _machineRepository.InsertOrUpdateAsync(schema, cancellationToken);
            return responseJson;
        }
    }
}
