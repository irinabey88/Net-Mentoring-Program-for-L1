using System.Data.Entity.ModelConfiguration;
using Common.Models;

namespace Dataccess.Configuration
{
    public class TerritoryConfiguration: EntityTypeConfiguration<Territory>
    {
        public TerritoryConfiguration()
        {
            ToTable("Territories");
            HasKey(p => p.TerritoryID);
            HasRequired(p => p.Region)
                .WithMany(x => x.Territories);

            Property(p => p.RegionID)
                .HasColumnName("RegionID");
            Property(p => p.TerritoryDescription)
                .HasMaxLength(50)
                .IsRequired();
        }
    }
}