using SupportHelper.Communication.Responses;
using SupportHelper.WinServices.Core.Models.ValueObjects;

namespace SupportHelper.WinServices.Core.Converters
{
    public static class NetworkBoardConvert
    {
        public static ICollection<NetworkBoardResponse> EntityToResponse(NetworkBoard[] networkBoard)
        {
            List<NetworkBoardResponse> response = [];
            foreach (var item in networkBoard)
            {
                response.Add(NetworkBoardResponse.Create(item.Description, item.Ipv4, item.Ipv6, item.MacAddress,
                    item.InUse));
            }
            return response;
        }

        public static ICollection<NetworkBoard> ResponseToEntity(ICollection<NetworkBoardResponse> networkBoard)
        {
            List<NetworkBoard> response = [];
            foreach (var item in networkBoard)
            {
                response.Add(NetworkBoard.Create(item.Description, item.Ipv4, item.Ipv6, item.MacAddress,
                    item.InUse));
            }
            return response;
        }
    }
}
