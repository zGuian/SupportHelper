using System.Net.NetworkInformation;

namespace SupportHelper.WinServices.Core.Models
{
    public class NetworkBoard
    {
        public string Name { get; private set; }
        public string Ipv4 { get; private set; }
        public string? Ipv6 { get; private set; }
        public string MacAddress { get; private set; }
        public bool InUse { get; private set; }

        private NetworkBoard()
        {
            Name = string.Empty;
            Ipv4 = string.Empty;
            Ipv6 = string.Empty;
            MacAddress = string.Empty;
        }

        public static NetworkBoard[] GetAllInformation()
        {
            var networkBoard = new HashSet<NetworkBoard>();
            var model = new NetworkBoard();
            var interfaces = NetworkInterface.GetAllNetworkInterfaces();
            foreach (var nic in interfaces)
            {
                if (nic.OperationalStatus == OperationalStatus.Down)
                {
                    model.InUse = false;
                    continue;
                }
            }
            return networkBoard.ToArray();
        }
    }
}
