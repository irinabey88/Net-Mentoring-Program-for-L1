using System.Data.Entity.ModelConfiguration;
using Common.Models;

namespace Dataccess.Configuration
{
    public class OrderConfiguration: EntityTypeConfiguration<Order>
    {
        public OrderConfiguration()
        {
            ToTable("Orders");
            HasKey(p => p.OrderID);
            HasOptional(p => p.Shipper)
                .WithMany(p => p.Orders)
                .HasForeignKey(p => p.ShipVia);
            HasOptional(p => p.Employee)
                .WithMany(p => p.Orders)
                .HasForeignKey(p => p.EmployeeID);

            Property(p => p.CustomerID)
                .IsFixedLength()
                .IsRequired();
            Property(p => p.OrderID)
                .HasColumnName("OrderID")
                .IsRequired();
            Property(p => p.OrderDate).IsOptional();
            Property(p => p.RequiredDate).IsOptional();
            Property(p => p.ShippedDate).IsOptional();

            Property(p => p.ShipVia).IsOptional();
            Property(p => p.Freight).IsOptional();
            Property(p => p.ShipName)
                .HasMaxLength(40)
                .IsOptional();
            Property(p => p.ShipAddress)
                .HasMaxLength(60)
                .IsOptional();
            Property(p => p.ShipCity)
                .HasMaxLength(15)
                .IsOptional();
            Property(p => p.ShipRegion)
                .HasMaxLength(15)
                .IsOptional();
            Property(p => p.ShipPostalCode)
                .HasMaxLength(10)
                .IsOptional();
            Property(p => p.ShipCountry)
                .HasMaxLength(15)
                .IsOptional();
        }
    }
}