namespace SupportHelper.Communication.Responses
{
    public record MachineInformationResponse
    {
        public string? IdServiceWindows { get; init; }
        public string Hostname { get; init; }
        public NetworkBoad[]? NetworkBoads { get; init; }

        public MachineInformationResponse(string hostname, string? idServiceWindows) 
        {
            Hostname = hostname;
            IdServiceWindows = idServiceWindows;
        }

        public MachineInformationResponse(string hostname, string? idServiceWindows, NetworkBoad[]? networkBoads)
        {
            IdServiceWindows = idServiceWindows;
            Hostname = hostname;
            NetworkBoads = networkBoads;
        }
    }

    public record NetworkBoad
    {
        public string? Ipv4 { get; private set; }
        public string? Ipv6 { get; private set; }
        public string? MacAddress { get; private set; }

        public NetworkBoad(string? ipv4, string? ipv6, string? macAddress)
        {
            Ipv4 = ipv4;
            Ipv6 = ipv6;
            MacAddress = macAddress;
        }
    }
}
