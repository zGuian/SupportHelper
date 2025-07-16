using Bogus;
using SupportHelper.Communication.Requests;

namespace SupportHelper.Test.Common.Utilities.Requests
{
    public class MachineInformationRequestBuilder
    {
        public static MachineInformationRequest Build()
        {
            var faker = new Faker("pt_BR");
            var hostname = faker.Random.String(15);
            var exchange = faker.Random.String(15);
            var queueReplyTo = faker.Random.String(15);

            var rabbitMq = new RabbitMQRequest(hostname, exchange, queueReplyTo);

            return new Faker<MachineInformationRequest>()
                .CustomInstantiator(f =>
                new MachineInformationRequest("GET_INFORMATION_MACHINE", rabbitMq));
        }
    }
}
