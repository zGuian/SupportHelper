using Bogus;
using SupportHelper.Communication.Requests;

namespace SupportHelper.Test.Common.Utilities.Requests
{
    public class RequestStatusMachineJsonBuilder
    {
        public static RequestStatusMachineJson Build()
        {
            var faker = new Faker("pt_BR");
            var hostname = faker.Random.String(15);

            return new RequestStatusMachineJson
            {
                Hostname = hostname,
            };
        }
    }
}
