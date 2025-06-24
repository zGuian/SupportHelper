namespace SupportHelper.Communication.Requests
{
    public sealed record RequestStatusToMachineJson
    {
        public required string Hostname { get; init; }
    }
}
