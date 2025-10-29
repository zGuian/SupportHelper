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
        , IUnitOfWork unitOfWork
        , IQueueUpdateSgpClient queue
        , ICacheTemp cache) : IMachineServices
    {
        private readonly IMachineRepositoryCommand _machineCommand = machineCommand;
        private readonly IMachineRepositoryQuery _machineQuery = machineQuery;
        private readonly IMachineSignalRServices _signalR = signalR;
        private readonly IQueueUpdateSgpClient _queue = queue;
        private readonly ICacheTemp _cache = cache;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<MachineDto> GetInformationMachineInDatabaseAsync(string hostname, CancellationToken ct = default)
        {
            var machine = await _machineQuery.GetByHostnameAsync(hostname, ct);
            return machine.Adapt<MachineDto>();
        }

        public async Task<MachineDto> GetInformationAndUpdateDatabaseAsync(string hostname, bool isRegistered, CancellationToken ct = default)
        {
            // SE NÃO TIVER REGISTRADO NÃO PRECISO BUSCAR CONNECTIONID NO BANCO.
            var connId = await _machineQuery.GetConnectionByHostnameAsync(hostname, ct);
            var responseJson = await _signalR.RequestStatusAsync(connId, ct);
            var machine = responseJson.Adapt<Machine>();
            if (isRegistered)
            {
                _machineCommand.Update(machine);
                _unitOfWork.Commit();
            }
            else
            {
                await _machineCommand.RegisterAsync(machine, ct);
                await _unitOfWork.CommitAsync();
            }
            return responseJson.Adapt<MachineDto>();
        }

        public async Task<MachineDto> GetInformationFirstConnection(string connId, CancellationToken ct = default)
        {
            var responseJson = await _signalR.RequestStatusAsync(connId, ct);
            var machine = responseJson.Adapt<Machine>();
            await _machineCommand.InsertOrUpdateNewConnectionsAsync(machine, ct);
            return responseJson.Adapt<MachineDto>();
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
