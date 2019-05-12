using System.Data.Entity.ModelConfiguration;
using Common.Models;

namespace Dataccess.Configuration
{
    public class RegionConfiguration: EntityTypeConfiguration<Region>
    {
        public RegionConfiguration()
        {
            ToTable("Region");
            HasKey(p => p.RegionID);
            HasMany(p => p.Territories)
                .WithRequired(p => p.Region)
                .WillCascadeOnDelete(false);

            Property(p => p.RegionID)
                .HasColumnName("RegionID")
                .IsRequired();
            Property(p => p.RegionDescription)
                .IsFixedLength()
                .HasMaxLength(50)
                .IsRequired();
        }
    }
}