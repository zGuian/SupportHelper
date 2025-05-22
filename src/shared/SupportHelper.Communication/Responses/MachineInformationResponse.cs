namespace SupportHelper.Communication.Responses
{
    public record MachineInformationResponse
    {
        public string? IdServiceWindows { get; init; }
        public required string Hostname { get; init; }
        public string? Ipv4 { get; init; }
    }
}
