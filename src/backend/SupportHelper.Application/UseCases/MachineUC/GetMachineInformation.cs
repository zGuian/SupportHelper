using SupportHelper.Application.Interfaces;
using SupportHelper.Communication.Requests;
using SupportHelper.Communication.Responses;
using SupportHelper.Domain.Entities;
using SupportHelper.Domain.Interfaces.MessageBrokerServices;
using System.Text;
using System.Text.Json;

namespace SupportHelper.Application.UseCases.MachineUC
{
    public class GetMachineInformation : IGetMachineInformation
    {
        private readonly IRabbitMQProducer _producer;
        public GetMachineInformation(IRabbitMQProducer producer) 
        {
            _producer = producer;
        }

        public async Task<MachineInformationResponse> ExecuteAsync(MachineInformationRequest request)
        {
            var json = JsonSerializer.Serialize(request);
            var headers = new Dictionary<string, string>();
            headers.TryAdd("Hostname", request.Hostname);
            headers.TryAdd("IPV4", request.Ipv4 ?? throw new ArgumentNullException());
            await _producer.Publisher(json, headers);
        }
    }
}
