using Microsoft.Win32;
using SupportHelper.Service.Domain.DTOs.Responses;
using SupportHelper.Service.Domain.Interface.UseCases;
using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace SupportHelper.Service.Domain.UseCases
{
    public class GetStatusMachineUseCase : IGetStatusMachineUseCase
    {
        private bool InUse { get; set; }
        private string Description { get; set; } = string.Empty;
        private string MacAddress { get; set; } = string.Empty;
        private string Ipv4 { get; set; } = string.Empty;
        public string Ipv6 { get; set; } = string.Empty;

        public ResponseStatusMachineJson Execute()
        {
            return new ResponseStatusMachineJson
            {
                Hostname = Environment.MachineName,
                DomainName = Environment.UserDomainName,
                CurrentUsername = Environment.UserName,
                OperationalSystem = Environment.OSVersion.VersionString,
                NetworkBoards = GetAllInformationNetwork(),
                UpTime = GetUpTime(),
                IsConnected = true,
                LastUpdate = DateTime.Now.ToString("dd/MM/yyy-HH:mm:ss")
            };
        }

        private static string GetUpTime()
        {
            var upTime = Environment.TickCount64;
            return TimeSpan.FromMilliseconds(upTime).ToString();
        }

        private IEnumerable<ResponseNetworkBoard> GetAllInformationNetwork()
        {
            var networkBoard = new HashSet<ResponseNetworkBoard>();
            var nics = NetworkInterface.GetAllNetworkInterfaces();
            foreach (var adpter in nics)
            {
                if (adpter.OperationalStatus != OperationalStatus.Up ||
                    adpter.NetworkInterfaceType == NetworkInterfaceType.Loopback)
                {
                    InUse = false;
                    continue;
                }
                Description = adpter.Description;
                MacAddress = adpter.GetPhysicalAddress().ToString();
                var props = adpter.GetIPProperties();
                foreach (var ip in props.UnicastAddresses)
                {
                    if (ip.Address.AddressFamily == AddressFamily.InterNetwork)
                    {
                        Ipv4 = ip.Address.ToString();
                    }
                    if (ip.Address.AddressFamily == AddressFamily.InterNetworkV6)
                    {
                        Ipv6 = ip.Address.ToString();
                    }
                }
                networkBoard.Add(ResponseNetworkBoard.Create(Description, Ipv4, Ipv6, MacAddress, InUse));
                CleanProperties();
            }
            return [.. networkBoard];
        }

        private void CleanProperties()
        {
            Description = string.Empty;
            MacAddress = string.Empty;
            Ipv4 = string.Empty;
            Ipv6 = string.Empty;
        }
    }
}
