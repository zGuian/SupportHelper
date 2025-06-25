namespace SupportHelper.Communication.Responses
{
    public record ResponseStatusMachineJson
    {
        public required bool IsConnected { get; init; }
        public required string Hostname { get; init; }
        public required string Ipv4 { get; init; }
        public bool? SgpIsRunning { get; init; }
    }
}
