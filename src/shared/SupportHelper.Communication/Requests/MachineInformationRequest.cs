namespace SupportHelper.Communication.Requests
{
    public record MachineInformationRequest
    {
        public required string Hostname { get; init; }
    }
}
