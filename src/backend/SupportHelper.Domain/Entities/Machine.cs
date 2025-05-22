namespace SupportHelper.Domain.Entities
{
    public class Machine
    {
        public string Id { get; private set; } = string.Empty;
        public string Ipv4 { get; private set; } = string.Empty;
        public string Ipv6 { get; private set; } = string.Empty;
        public string[] MacAddress { get; private set; } = [];
        
    }
}
