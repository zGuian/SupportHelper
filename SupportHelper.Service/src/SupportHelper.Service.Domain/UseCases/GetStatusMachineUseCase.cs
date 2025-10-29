using SupportHelper.Service.Domain.DTOs.Responses;
using SupportHelper.Service.Domain.Interface.UseCases;
using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace SupportHelper.Service.Domain.UseCases
{
    public class GetStatusMachineUseCase : IGetStatusMachineUseCase
    {
        public ResponseStatusMachineJson Execute()
        {
            return new ResponseStatusMachineJson
            {
                Hostname = Environment.MachineName,
                DomainName = Environment.UserDomainName,
                CurrentUsername = Environment.UserName,
                OperationalSystem = Environment.OSVersion.VersionString,
                NetworkBoards = GetAllInformation(),
                UpTime = GetUpTime(),
                IsConnected = true,
                LastUpdate = DateTimeOffset.Now.LocalDateTime.ToString()
            };
        }

        private static string GetUpTime()
        {
            var upTime = Environment.TickCount64;
            return TimeSpan.FromMilliseconds(upTime).ToString();
        }

        public static IEnumerable<ResponseNetworkBoard> GetAllInformation()
        {
            var nics = NetworkInterface.GetAllNetworkInterfaces();
            var filtered = nics
                .Where(a =>
                    a.OperationalStatus != OperationalStatus.Unknown &&
                    a.NetworkInterfaceType != NetworkInterfaceType.Loopback &&
                    a.NetworkInterfaceType != NetworkInterfaceType.Tunnel &&
                    a.NetworkInterfaceType != NetworkInterfaceType.Unknown &&
                    !a.Description.Contains("Virtual", StringComparison.OrdinalIgnoreCase) &&
                    !a.Description.Contains("Hyper-V", StringComparison.OrdinalIgnoreCase) &&
                    !a.Description.Contains("WAN Miniport", StringComparison.OrdinalIgnoreCase)
                );

            var result = new HashSet<ResponseNetworkBoard>();
            var macs = new HashSet<string>();

            foreach (var adapter in filtered)
            {
                var mac = adapter.GetPhysicalAddress().ToString();
                if (string.IsNullOrWhiteSpace(mac) || mac.Length < 8)
                    continue;

                if (!macs.Add(mac))
                    continue;

                var entity = new ResponseNetworkBoard
                {
                    Description = adapter.Description,
                    MacAddress = mac,
                    InUse = adapter.OperationalStatus == OperationalStatus.Up
                };

                var props = adapter.GetIPProperties();
                foreach (var ip in props.UnicastAddresses)
                {
                    if (ip.Address.AddressFamily == AddressFamily.InterNetwork)
                        entity.Ipv4 = ip.Address.ToString();
                    else if (ip.Address.AddressFamily == AddressFamily.InterNetworkV6)
                        entity.Ipv6 = ip.Address.ToString();
                }

                if (!string.IsNullOrEmpty(entity.Ipv4))
                    entity.InUse = true;

                result.Add(entity);
            }
            return result;
        }
    }
}
