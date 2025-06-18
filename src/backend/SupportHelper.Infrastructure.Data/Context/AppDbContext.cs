using Microsoft.EntityFrameworkCore;
using SupportHelper.Infrastructure.Data.Mapping.Database;

namespace SupportHelper.Infrastructure.Data.Context
{
    public partial class AppDbContext(DbContextOptions<AppDbContext> opts) : DbContext(opts)
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new MachineMap());
        }
    }
}
