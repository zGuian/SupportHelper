namespace SupportHelper.Communication.Requests
{
    public record MachineInformationRequest
    {
        public required string Hostname { get; init; }
        public string? Ipv4 { get; init; }
    }
}
