using Microsoft.EntityFrameworkCore;
using SupportHelper.API.Domain.Entities;
using SupportHelper.API.Infra.Data.Context.Map;

namespace SupportHelper.API.Infra.Data.Context
{
    public class AppDbContext(DbContextOptions<AppDbContext> opts) : DbContext(opts)
    {
        public DbSet<Machine> Machines { get; set; } = null!;
        public DbSet<NetworkBoard> NetworkBoards { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new MachineMap());
            modelBuilder.ApplyConfiguration(new NetworkBoardMap());
            base.OnModelCreating(modelBuilder);
        }
    }
}
