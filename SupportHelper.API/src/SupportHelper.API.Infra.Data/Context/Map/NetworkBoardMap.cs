using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SupportHelper.API.Domain.Entities;

namespace SupportHelper.API.Infra.Data.Context.Map
{
    internal class NetworkBoardMap : IEntityTypeConfiguration<NetworkBoard>
    {
        public void Configure(EntityTypeBuilder<NetworkBoard> builder)
        {
            builder.ToTable("TB_NETWORKBOARD");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id)
                .HasColumnName("COL_ID")
                .HasColumnOrder(0)
                .HasColumnType("integer")
                .IsRequired();

            builder.Property(x => x.MachineId)
                .HasColumnName("FK_MACHINE")
                .HasColumnOrder(1)
                .IsRequired(true);

            builder.Property(x => x.Ipv4)
                .HasColumnName("COL_IPV4")
                .HasColumnOrder(2)
                .HasColumnType("CHAR(15)")
                .IsRequired(false);

            builder.HasAlternateKey(x => x.MacAddress);
            builder.Property(x => x.MacAddress)
                .HasColumnName("COL_MACADRESS")
                .HasColumnOrder(3)
                .HasColumnType("CHAR(12)")
                .IsRequired(true);

            builder.Property(x => x.InUse)
                .HasColumnName("COL_INUSE")
                .HasColumnOrder(4)
                .HasColumnType("BIT")
                .IsRequired(true);

            builder.Property(x => x.Description)
                .HasColumnName("COL_DESCRIPTION")
                .HasColumnOrder(5)
                .HasMaxLength(50)
                .IsRequired(true);

            builder.Property(x => x.Ipv6)
                .HasColumnName("COL_IPV6")
                .HasColumnOrder(6)
                .HasColumnType("VARCHAR(30)")
                .IsRequired(false);
        }
    }
}

