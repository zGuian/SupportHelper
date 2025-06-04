using MassTransit;
using SupportHelper.Communication.Responses;

namespace SupportHelper.Infrastructure.MQServices.Response
{
    public class MachineResponseConsumer<T> : IConsumer<ResponseBase<T>>
    {
        public async Task Consume(ConsumeContext<ResponseBase<T>> context)
        {
            var correlationId = context.CorrelationId;
            var response = context.Message;
            await Task.CompletedTask;
        }
    }
}
