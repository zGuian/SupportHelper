using MassTransit;
using SupportHelper.Communication.Responses;
using SupportHelper.Domain.Entities;
using SupportHelper.Domain.Interfaces.Repositories;

namespace SupportHelper.Infrastructure.MQServices.Responses
{
    public class MachineResponseConsumer : IConsumer<ResponseBase<Machine>>
    {
        private readonly IMachineRepository _machineRepository;

        public MachineResponseConsumer(IMachineRepository machineRepository)
        {
            _machineRepository = machineRepository;
        }

        public async Task Consume(ConsumeContext<ResponseBase<Machine>> context)
        {
            var response = context.Message;
            if (response?.Value != null && response.IsSuccess)
            {
                await _machineRepository.InsertMachineByProcedure(response.Value);
            }
        }
    }
}
