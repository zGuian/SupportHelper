using SupportHelper.Application.Interfaces;
using SupportHelper.Communication.Responses;
using SupportHelper.Domain.Aggregates;
using SupportHelper.Domain.Interfaces.Repositories.Database;
using SupportHelper.Domain.Interfaces.Repositories.Memory;
using SupportHelper.Domain.Interfaces.SignalRContext;

namespace SupportHelper.Application.UseCases.MachineUC
{
    public class RequestStatusMachineUseCase : IRequestStatusMachineUseCase
    {
        private readonly IMachineSignalRServices _signalR;
        private readonly IMachineRepository _machineRepository;
        private readonly IConnectionMemoryRepository _connectionMemoryRepository;

        public RequestStatusMachineUseCase(IMachineSignalRServices signalR, IMachineRepository machineRepository,
            IConnectionMemoryRepository connectionMemoryRepository)
        {
            _signalR = signalR;
            _connectionMemoryRepository = connectionMemoryRepository;
            _machineRepository = machineRepository;
        }

        public async Task<ResponseStatusMachineJson> ExecuteAsync(string hostname)
        {
            _connectionMemoryRepository.GetConnectionId(hostname, out var connId);
            if (string.IsNullOrWhiteSpace(connId))
            {
                var responseBase = await _machineRepository.GetByHostnameAsync(hostname);
                connId = responseBase.SignalR.ConnectionId;
            }
            var responseJson = await _signalR.RequestStatusAsync(connId);
            var schema = MachineSchemaJson.Create(responseJson, connId);
            await _machineRepository.InsertOrUpdateAsync(schema);
            return responseJson;
        }
    }
}
