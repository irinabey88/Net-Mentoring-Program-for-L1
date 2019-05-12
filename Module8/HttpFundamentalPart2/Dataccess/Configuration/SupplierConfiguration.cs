using System.Data.Entity.ModelConfiguration;
using Common.Models;

namespace Dataccess.Configuration
{
    public class SupplierConfiguration: EntityTypeConfiguration<Supplier>
    {
        public SupplierConfiguration()
        {
            ToTable("Suppliers");
            HasKey(p => p.SupplierID);

            Property(p => p.SupplierID)
                .HasColumnName("SupplierID");
            Property(p => p.CompanyName)
                .HasMaxLength(40)
                .IsRequired();
            Property(p => p.ContactName)
                .HasMaxLength(30)
                .IsOptional();
            Property(p => p.ContactTitle)
                .HasMaxLength(30)
                .IsOptional();
            Property(p => p.Address)
                .HasMaxLength(60)
                .IsOptional();
            Property(p => p.City)
                .HasMaxLength(15)
                .IsOptional();
            Property(p => p.Region)
                .HasMaxLength(15)
                .IsOptional();
            Property(p => p.PostalCode)
                .HasMaxLength(10)
                .IsOptional();
            Property(p => p.Country)
                .HasMaxLength(15)
                .IsOptional();
            Property(p => p.Phone)
                .HasMaxLength(24)
                .IsOptional();
            Property(p => p.Fax)
                .HasMaxLength(24)
                .IsOptional();
            Property(p => p.HomePage)
                .HasColumnType("ntext")
                .IsOptional();
        }
    }
}