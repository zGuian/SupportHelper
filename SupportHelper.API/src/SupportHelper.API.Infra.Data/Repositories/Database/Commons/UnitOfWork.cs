using SupportHelper.API.Domain.Interfaces.Repositories.Commons;
using SupportHelper.API.Infra.Data.Context;

namespace SupportHelper.API.Infra.Data.Repositories.Database.Commons
{
    public class UnitOfWork(AppDbContext context) : IUnitOfWork
    {
        private readonly AppDbContext _context = context;

        public void Commit()
        {
            _context.SaveChanges();
        }

        public async Task CommitAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
