using System.Data.Entity.ModelConfiguration;
using Common.Models;

namespace Dataccess.Configuration
{
    public class ShipperConfiguration: EntityTypeConfiguration<Shipper>
    {
        public ShipperConfiguration()
        {
            ToTable("Shippers");
            HasKey(p => p.ShipperID);
            HasMany(p => p.Orders)
                .WithOptional(p => p.Shipper)
                .HasForeignKey(p => p.ShipVia);

            Property(p => p.ShipperID)
                .HasColumnName("ShipperID");
            Property(p => p.CompanyName)
                .HasMaxLength(40)
                .IsRequired();
            Property(p => p.Phone)
                .HasMaxLength(24)
                .IsRequired();
        }
    }
}