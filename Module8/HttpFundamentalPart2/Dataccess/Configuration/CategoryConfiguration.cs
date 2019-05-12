using System.Data.Entity.ModelConfiguration;
using Common.Models;

namespace Dataccess.Configuration
{
    public class CategoryConfiguration: EntityTypeConfiguration<Category>
    {
        public CategoryConfiguration()
        {
            ToTable("Categories");

            HasKey(p => p.CategoryID);

            Property(p => p.CategoryName)
                .HasMaxLength(15)
                .IsRequired();
            Property(p => p.Description)
                .HasColumnType("ntext")
                .IsOptional();
            Property(p => p.Picture)
                .HasColumnType("image")
                .IsOptional();
        }
    }
}