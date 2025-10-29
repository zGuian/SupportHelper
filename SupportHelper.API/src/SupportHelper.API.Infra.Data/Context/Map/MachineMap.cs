using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SupportHelper.API.Domain.Entities;

namespace SupportHelper.API.Infra.Data.Context.Map
{
    internal class MachineMap : IEntityTypeConfiguration<Machine>
    {
        public void Configure(EntityTypeBuilder<Machine> builder)
        {
            builder.ToTable("TB_MACHINE");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id)
                .HasColumnName("COL_ID")
                .ValueGeneratedOnAdd()
                .HasColumnOrder(0);

            builder.HasIndex(x => x.Hostname).IsUnique();
            builder.HasAlternateKey(x => x.Hostname);
            builder.Property(x => x.Hostname)
                .HasColumnName("COL_HOSTNAME")
                .HasMaxLength(15)
                .HasColumnOrder(1)
                .HasColumnType("CHAR(14)")
                .IsRequired(true);

            builder.Property(x => x.IsConnected)
                .HasColumnName("COL_ISCONNECTED")
                .HasColumnOrder(2)
                .HasColumnType("BIT")
                .IsRequired(true);

            builder.Property(x => x.SgpIsRunning)
                .HasColumnName("COL_SGPISRUNNING")
                .HasColumnOrder(3)
                .HasColumnType("BIT")
                .IsRequired(true);

            builder.HasMany(x => x.NetworkBoards)
                .WithOne(x => x.Machine)
                .HasForeignKey(x => x.MachineId);

            builder.Property(x => x.CurrentUsername)
                .HasColumnName("COL_CURRENTUSERNAME")
                .HasMaxLength(20)
                .HasColumnOrder(4)
                .HasColumnType("VARCHAR(20)")
                .IsRequired(true);

            builder.Property(x => x.DomainName)
                .HasColumnName("COL_DOMAINNAME")
                .HasMaxLength(25)
                .HasColumnOrder(5)
                .HasColumnType("VARCHAR(25)")
                .IsRequired(true);

            builder.Property(x => x.UpTime)
                .HasColumnName("COL_UPTIME")
                .HasColumnType("VARCHAR(20)")
                .HasColumnOrder(6)
                .IsRequired();

            builder.Property(x => x.OperationalSystem)
                .HasMaxLength(35)
                .HasColumnName("COL_OPERATIONALSYSTEM")
                .HasColumnOrder(7)
                .HasColumnType("VARCHAR(30)")
                .IsRequired(true);

            builder.OwnsOne(x => x.SignalR, signal =>
            {
                signal.Property(p => p.ConnectionId)
                    .HasColumnName("COL_SIGNALR_CONNECTIONID")
                    .HasColumnType("VARCHAR(100)")
                    .HasColumnOrder(8)
                    .HasMaxLength(100)
                    .IsRequired(true);

                signal.Property(p => p.IsActive)
                    .HasColumnName("COL_SIGNALR_ISACTIVE")
                    .HasColumnType("BIT")
                    .HasColumnOrder(9)
                    .IsRequired(true);
            });

            builder.Property(x => x.LastUpdate)
                .HasColumnName("COL_LASTUPDATE")
                .HasColumnType("DATETIME2")
                .HasColumnOrder(10)
                .IsRequired(true);
        }
    }
}
