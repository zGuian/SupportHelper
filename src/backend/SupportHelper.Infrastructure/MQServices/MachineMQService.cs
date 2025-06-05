using MassTransit;
using Microsoft.Extensions.Configuration;
using SupportHelper.Communication.Requests;
using SupportHelper.Domain.Interfaces.MQServices;
using System.Net.Mime;

namespace SupportHelper.Infrastructure.MQServices
{
    public class MachineMQService : IMachineMQServices
    {
        private readonly IBus _bus;
        private readonly IConfiguration _configuration;

        public MachineMQService(IBus bus, IConfiguration configuration)
        {
            _bus = bus;
            _configuration = configuration;
        }

        public async Task PublishGetInformationAsync(MachineInformationRequest request)
        {
            var routingKey = $"machine.worker.{request.RabbitMQRequest.Hostname}";

            var endpoint = await _bus.GetSendEndpoint(new Uri(
                $"exchange:{request.RabbitMQRequest.Exchange}?bind=true&bindRoutingKey={routingKey}"));

            await endpoint.Send(request, context =>
            {
                context.ContentType = new ContentType("application/json");
                context.ResponseAddress = new Uri(_configuration["RabbitMQ:ConfigExchange:ReplyToDefault"]!);
                context.Headers.Set("routingKey", routingKey);
            });
        }
    }
}
