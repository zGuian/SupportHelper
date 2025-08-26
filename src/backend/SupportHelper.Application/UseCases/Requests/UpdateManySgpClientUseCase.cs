using SupportHelper.Application.Interfaces;
using SupportHelper.Application.Utils;
using SupportHelper.Communication.Requests;
using SupportHelper.Communication.Responses;
using SupportHelper.Domain.Interfaces.ApplicationContext;
using SupportHelper.Domain.Interfaces.Repositories.Database;
using SupportHelper.Domain.Interfaces.SignalRContext;

namespace SupportHelper.Application.UseCases.Requests
{
    public sealed class UpdateManySgpClientUseCase : IUpdateManySgpClientUseCase
    {
        private readonly IQueueUpdateSgpClient _queue;
        private readonly IMachineSignalRServices _signalRServices;
        private readonly IMachineRepository _machineRepository;

        public UpdateManySgpClientUseCase(IQueueUpdateSgpClient queue, IMachineSignalRServices signalRServices, 
            IMachineRepository machineRepository)
        {
            _queue = queue;
            _signalRServices = signalRServices;
            _machineRepository = machineRepository;
        }

        public async Task<IEnumerable<ResponseBase<ResponseUpdateSgpClientJson>>> ExecuteAsync(IEnumerable<RequestUpdateSgpClientJson> requests, CancellationToken ct = default)
        {
            Verify(requests);
            var manyConnId = await _machineRepository.GetManyConnectionAsync(requests, ct);
            foreach (var item in manyConnId)
            {
                _queue.Enqueue(item.Key, item.Value);
            }
            var responses = await _signalRServices.UpdateManySgpClientAsync(_queue, ct);
            return responses;
        }

        private void Verify(IEnumerable<RequestUpdateSgpClientJson> requests)
        {
            if (requests == null)
            {
                return;
            }
        }
    }
}
