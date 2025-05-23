using SupportHelper.Application.Interfaces;
using SupportHelper.Communication.Requests;
using SupportHelper.Communication.Responses;
using SupportHelper.Domain.Interfaces.MessageBrokerServices;
using System.Text.Json;

namespace SupportHelper.Application.UseCases.MachineUC
{
    public class GetMachineInformation : IGetMachineInformation
    {
        private readonly IProducerServices _producer;
        private readonly IConsumeServices _consume;

        public GetMachineInformation(IProducerServices producer, IConsumeServices consume)
        {
            _producer = producer;
            _consume = consume;
        }

        public async Task<MachineInformationResponse> ExecuteAsync(MachineInformationRequest request)
        {
            var message = JsonSerializer.Serialize(request);
            var headers = new Dictionary<string, string>();
            headers.TryAdd("Hostname", request.Hostname);
            headers.TryAdd("IPV4", request.Ipv4 ?? throw new ArgumentNullException());
            _producer.PublishMessage(message);
            return JsonSerializer.Deserialize<MachineInformationResponse>(_consume.ConsumeMessage())
                ?? throw new ArgumentNullException();
        }
    }
}
