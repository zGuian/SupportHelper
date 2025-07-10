namespace SupportHelper.Communication.Requests
{
    public sealed record RequestStatusMachineJson
    {
        public required string Hostname { get; init; }
    }
}
