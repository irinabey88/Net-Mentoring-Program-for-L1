using System.Data.Entity.ModelConfiguration;
using Common.Models;

namespace Dataccess.Configuration
{
    public class CustomerConfiguration: EntityTypeConfiguration<Customer>
    {
        public CustomerConfiguration()
        {
            ToTable("Customers");
            HasKey(p => p.CustomerID);

            Property(p => p.CustomerID)
                .IsFixedLength()
                .HasMaxLength(5)
                .HasColumnName("CustomerID");
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
        }
    }
}