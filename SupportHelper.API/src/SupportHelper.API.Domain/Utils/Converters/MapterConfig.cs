using Mapster;
using SupportHelper.API.Domain.DTOs.Client;
using SupportHelper.API.Domain.DTOs.Responses;
using SupportHelper.API.Domain.Entities;
using SupportHelper.API.Domain.Entities.ValueObjects;

namespace SupportHelper.API.Domain.Utils.Converters
{
    public static class MapterConfig
    {
        public static void Configure()
        {
            MachineMaps();
        }

        private static void MachineMaps()
        {
            TypeAdapterConfig<ResponseStatusMachineJson, Machine>.NewConfig()
                .ConstructUsing(json => new Machine(
                      json.Hostname
                    , json.CurrentUsername
                    , json.DomainName
                    , json.OperationalSystem
                    , json.NetworkBoards.Adapt<IEnumerable<NetworkBoard>>()
                    , json.IsConnected
                    , json.UpTime
                    , new SignalR(string.Empty, false)));

            TypeAdapterConfig<InfoMachineClient, Machine>.NewConfig()
                .ConstructUsing(json => new Machine(
                    json.Hostname
                    , json.CurrentUsername
                    , json.DomainName
                    , json.OperationalSystem
                    , json.NetworkBoardsJ.Select(x => NetworkBoard.Create(x.Description, x.Ipv4, x.Ipv6, x.MacAddress, x.InUse))
                    , json.IsConnected
                    , json.UpTime
                    , json.SignalR));
        }
    }
}
