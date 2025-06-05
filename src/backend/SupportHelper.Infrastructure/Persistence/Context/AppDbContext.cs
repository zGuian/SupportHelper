using Microsoft.EntityFrameworkCore;
using SupportHelper.Infrastructure.Persistence.Context.MapperDB;

namespace SupportHelper.Infrastructure.Persistence.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> context) : base(context)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new MachineMap());
        }
    }
}
