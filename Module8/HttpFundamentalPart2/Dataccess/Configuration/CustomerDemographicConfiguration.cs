using System.Data.Entity.ModelConfiguration;
using Common.Models;

namespace Dataccess.Configuration
{
    public class CustomerDemographicConfiguration: EntityTypeConfiguration<CustomerDemographic>
    {
        public CustomerDemographicConfiguration()
        {
            ToTable("CustomerDemographics");
            HasKey(p => p.CustomerTypeID);
            HasMany(p => p.Customers)
                .WithMany(p => p.CustomerDemographics)
                .Map(m => m.ToTable("CustomerCustomerDemo")
                .MapLeftKey("CustomerTypeID")
                .MapRightKey("CustomerID"));


            Property(p => p.CustomerTypeID)
                .IsFixedLength()
                .HasMaxLength(10)
                .HasColumnName("CustomerTypeID");
            Property(p => p.CustomerDesc)
                .HasColumnType("ntext")
                .IsOptional();
        }
    }
}