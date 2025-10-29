using SupportHelper.API.Domain.Entities;

namespace SupportHelper.API.Domain.Interfaces.Repositories
{
    public interface INetworkBoardRepositoryCommand
    {
        Task RegisterNetworkBoard(IEnumerable<NetworkBoard> networkBoards);
    }
}
