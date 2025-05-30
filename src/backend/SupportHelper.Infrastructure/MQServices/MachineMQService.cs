using MassTransit;
using SupportHelper.Communication.Requests;
using SupportHelper.Domain.Interfaces.MQServices;
using System.Net.Mime;
using System.Text.Unicode;

namespace SupportHelper.Infrastructure.MQServices
{
    public class MachineMQService : IMachineMQServices
    {
        private readonly ISendEndpointProvider _sendEndpointProvider;

        public MachineMQService(ISendEndpointProvider sendEndpointProvider)
        {
            _sendEndpointProvider = sendEndpointProvider;
        }

        public async Task<Guid> PublishGetMachineInformation(MachineInformationRequest request)
        {
            var routingKey = $"machine.{request.RabbitMQRequest.Hostname}";

            var endpoint = await _sendEndpointProvider.GetSendEndpoint(new Uri(
                $"exchange:{request.RabbitMQRequest.Exchange}?bind=true&bindRoutingKey={routingKey}"));

            var correlationId = Guid.NewGuid();
            var contentType = new ContentType("application/json");
            await endpoint.Send(request, context =>
            {
                context.Headers.Set("routingKey", routingKey);
                context.ContentType = contentType;
                context.CorrelationId = correlationId;
                context.ResponseAddress = new Uri($"queue:{request.RabbitMQRequest.ReplyToQueueName}");
            });

            return correlationId;
        }
    }
}
