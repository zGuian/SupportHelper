using SupportHelper.Communication.Responses;
using SupportHelper.WinServices.Core.Models.ValueObjects;

namespace SupportHelper.WinServices.Core.Converters
{
    public static class NetworkBoardConvert
    {
        public static IEnumerable<NetworkBoardResponse> EntityToResponse(IEnumerable<NetworkBoard> networkBoard)
        {
            var arrayResponse = new NetworkBoardResponse[networkBoard.Count()];
            var arrayEntity = networkBoard.ToArray();
            for (var i = 0; i < networkBoard.Count(); i++)
            {
                var item = arrayEntity[i];
                arrayResponse[i] = NetworkBoardResponse.Create(item.Description, item.Ipv4, item.Ipv6, item.MacAddress, item.InUse);
            }
            return arrayResponse;
        }

        public static IEnumerable<NetworkBoard> ResponseToEntity(IEnumerable<NetworkBoardResponse> networkBoardResponse)
        {
            var arrayEntity = new NetworkBoard[networkBoardResponse.Count()];
            var arrayResponse = networkBoardResponse.ToArray();
            for (int i = 0; i < networkBoardResponse.Count(); i++)
            {
                var item = arrayResponse[i];
                arrayEntity[i] = NetworkBoard.Create(item.Description, item.Ipv4, item.Ipv6, item.MacAddress, item.InUse);
            }
            return arrayEntity;
        }
    }
}
