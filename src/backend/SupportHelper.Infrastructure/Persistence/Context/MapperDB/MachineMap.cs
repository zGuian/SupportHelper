using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SupportHelper.Domain.Entities;

namespace SupportHelper.Infrastructure.Persistence.Context.MapperDB
{
    public class MachineMap : IEntityTypeConfiguration<Machine>
    {
        public void Configure(EntityTypeBuilder<Machine> builder)
        {
            builder.ToTable("machine");
            builder.HasKey(m => m.Id);

            builder.Property(m => m.Id)
                .HasColumnName("id")
                .HasColumnOrder(0)
                .IsRequired(true);

            builder.Property(m => m.Hostname)
                .HasColumnName("hostname")
                .HasColumnOrder(1)
                .HasMaxLength(50)
                .IsRequired(true);

            builder.Property(m => m.CurrentUsername)
                .HasColumnName("current_username")
                .HasColumnOrder(2)
                .HasMaxLength(50)
                .IsRequired(true);

            builder.Property(m => m.DomainName)
                .HasColumnName("domain_name")
                .HasColumnOrder(3)
                .HasMaxLength(50)
                .IsRequired(true);

            builder.Property(m => m.OperationalSystem)
                .HasColumnName("operational_system")
                .HasColumnOrder(4)
                .HasMaxLength(50)
                .IsRequired(true);

            builder.Ignore(m => m.NetworkBoards);

        }
    }
}
