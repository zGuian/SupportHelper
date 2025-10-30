using Mapster;
using SupportHelper.API.Domain.DTOs.Entities;
using SupportHelper.API.Domain.DTOs.Generics;
using SupportHelper.API.Domain.DTOs.Requests;
using SupportHelper.API.Domain.DTOs.Responses;
using SupportHelper.API.Domain.Entities;
using SupportHelper.API.Domain.Interfaces.Repositories;
using SupportHelper.API.Domain.Interfaces.Repositories.Commons;
using SupportHelper.API.Domain.Interfaces.Services;

namespace SupportHelper.API.Domain.Services
{
    public class MachineServices(IMachineRepositoryCommand machineCommand
        , IMachineRepositoryQuery machineQuery
        , IMachineSignalRServices signalR
        , IQueueUpdateSgpClient queue
        , IUnitOfWork unitOfWork
        , ICacheTemp cache) : IMachineServices
    {
        private readonly IMachineRepositoryCommand _machineCommand = machineCommand;
        private readonly IMachineRepositoryQuery _machineQuery = machineQuery;
        private readonly IMachineSignalRServices _signalR = signalR;
        private readonly IQueueUpdateSgpClient _queue = queue;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly ICacheTemp _cache = cache;

        public async Task<MachineDto> GetInformationMachineInDatabaseAsync(string hostname, CancellationToken ct = default)
        {
            var machine = await _machineQuery.GetByHostnameAsync(hostname, ct);
            return machine.Adapt<MachineDto>();
        }

        public async Task<MachineDto> GetInformationAndUpdateDatabaseAsync(string hostname, CancellationToken ct = default)
        {
            var connId = await _machineQuery.GetConnectionByHostnameAsync(hostname, ct);

            var responseTask = _signalR.RequestStatusAsync(connId, ct);
            var idTask = _machineQuery.GetIdByHostnameAsync(hostname);
            await Task.WhenAll(responseTask, idTask);

            var responseJson = await responseTask;
            var idMachine = await idTask;

            var machine = responseJson.Adapt<Machine>();
            machine.PrepareForEntity(idMachine);
            await _machineCommand.UpdateAsync(machine);
            await _unitOfWork.CommitAsync();

            return responseJson.Adapt<MachineDto>();
        }

        public async Task RegisterConnectionAsync(string hostname, string connId, CancellationToken ct = default)
        {
            var existTask = _machineQuery.ExistHostname(hostname);
            var statusTask = _signalR.RequestStatusAsync(connId, ct);
            await Task.WhenAll(existTask, statusTask);

            var existHostname = await existTask;
            var infoMachine = await statusTask;
            var machine = infoMachine.Adapt<Machine>();

            if (existHostname)
            {
                await _machineQuery.GetIdByHostnameAsync(hostname);

                machine.PrepareForEntity(await _machineQuery.GetIdByHostnameAsync(hostname));
                await _machineCommand.UpdateAsync(machine);
                await _unitOfWork.CommitAsync();
                return;
            }

            machine.ResetId();
            await _machineCommand.RegisterAsync(machine, ct);
            await _unitOfWork.CommitAsync();
        }

        public async Task<FileDataDto> GetLogsSgpClientAsync(RequestLogsSgpClientJson request, CancellationToken ct = default)
        {
            var connId = await _machineQuery.GetConnectionByHostnameAsync(request.Hostname, ct);
            var requestId = await _signalR.GetLogSgpClientAsync(connId, request, ct);
            try
            {
                return await _cache.GetFileAsync(requestId);
            }
            finally
            {
                await _cache.DeleteFileAsync(requestId);
            }
        }

        public async Task<IEnumerable<ResponseBase<ResponseUpdateSgpClientJson>>> UpdateManySgpClientAsync(
            IEnumerable<RequestUpdateSgpClientJson> requests
            , CancellationToken ct = default)
        {
            ArgumentNullException.ThrowIfNull(requests);
            var manyConnId = await _machineQuery.GetManyConnectionAsync(requests, ct);
            foreach (var item in manyConnId)
            {
                _queue.Enqueue(item.Key, item.Value);
            }
            var responses = await _signalR.UpdateManySgpClientAsync(_queue, ct);
            return responses;
        }

        public async Task<ResponseUpdateSgpClientJson> UpdateOnlySgpClientAsync(RequestUpdateSgpClientJson requestJson
            , CancellationToken ct = default)
        {
            var connectionId = await _machineQuery.GetConnectionByHostnameAsync(requestJson.Hostname, ct);
            var response = await _signalR.UpdateSgpClientAsync(connectionId, requestJson, ct);
            return response;
        }
    }
}
