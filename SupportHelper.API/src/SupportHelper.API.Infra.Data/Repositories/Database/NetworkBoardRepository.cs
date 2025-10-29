using SupportHelper.API.Domain.Entities;
using SupportHelper.API.Domain.Interfaces.Repositories;
using SupportHelper.API.Infra.Data.Context;

namespace SupportHelper.API.Infra.Data.Repositories.Database
{
    public class NetworkBoardRepository(AppDbContext context) : INetworkBoardRepositoryCommand, INetworkBoardRepositoryQuery
    {
        private readonly AppDbContext _context = context;

        public async Task RegisterNetworkBoard(IEnumerable<NetworkBoard> networkBoards)
        {
            foreach (var item in networkBoards)
            {
                _context.Add(item);
            }
            await _context.SaveChangesAsync();
        }
    }
}
