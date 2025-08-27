namespace SupportHelper.FrontEnd.MVC.Models.ValueObjects
{
    public struct NetworkBoardVO
    {
        public string Description { get; private set; }
        public string Ipv4 { get; private set; }
        public string? Ipv6 { get; private set; }
        public string MacAddress { get; private set; }
        public bool InUse { get; private set; }
    }
}