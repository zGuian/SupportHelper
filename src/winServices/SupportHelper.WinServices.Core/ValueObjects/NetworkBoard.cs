using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace SupportHelper.WinServices.Core.ValueObjects
{
    public class NetworkBoard
    {
        public string Description { get; private set; }
        public string Ipv4 { get; private set; }
        public string? Ipv6 { get; private set; }
        public string MacAddress { get; private set; }
        public bool InUse { get; private set; }

        private NetworkBoard()
        {
            Description = string.Empty;
            Ipv4 = string.Empty;
            Ipv6 = string.Empty;
            MacAddress = string.Empty;
            InUse = true;
        }

        public static NetworkBoard[] GetAllInformation()
        {
            var networkBoard = new HashSet<NetworkBoard>();
            var nics = NetworkInterface.GetAllNetworkInterfaces();
            foreach (var adpter in nics)
            {
                var entity = new NetworkBoard();
                if (adpter.OperationalStatus != OperationalStatus.Up ||
                    adpter.NetworkInterfaceType == NetworkInterfaceType.Loopback)
                {
                    entity.InUse = false;
                    continue;
                }
                entity.Description = adpter.Description;
                entity.MacAddress = adpter.GetPhysicalAddress().ToString();
                var props = adpter.GetIPProperties();
                foreach (var ip in props.UnicastAddresses)
                {
                    if (ip.Address.AddressFamily == AddressFamily.InterNetwork)
                    {
                        entity.Ipv4 = ip.Address.ToString();
                    }
                    if (ip.Address.AddressFamily == AddressFamily.InterNetworkV6)
                    {
                        entity.Ipv6 = ip.Address.ToString();
                    }
                }
                networkBoard.Add(entity);
            }
            return [..networkBoard];
        }
    }
}
