namespace SupportHelper.Communication.Requests
{
    public sealed record RequestMachineInformationJson
    {
        public required string Hostname { get; init; }
    }
}
