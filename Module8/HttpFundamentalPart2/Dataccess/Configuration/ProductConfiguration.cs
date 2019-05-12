using System.Data.Entity.ModelConfiguration;
using Common.Models;

namespace Dataccess.Configuration
{
    public class ProductConfiguration: EntityTypeConfiguration<Product>
    {
        public ProductConfiguration()
        {
            ToTable("Products");
;            HasKey(p => p.ProductID);
            HasOptional(p => p.Category)
                .WithMany(p => p.Products);
            HasOptional(p => p.Supplier)
                .WithMany(p => p.Products);
            HasMany(e => e.OrderDetails)
                .WithRequired(e => e.Product)
                .WillCascadeOnDelete(false);

            Property(p => p.ProductID)
                .HasColumnName("ProductID")
                .IsRequired();
            Property(p => p.CategoryID)
                .HasColumnName("CategoryID")
                .IsOptional();
            Property(p => p.SupplierID)
                .HasColumnName("SupplierID")
                .IsOptional();
            Property(p => p.ProductName)
                .HasMaxLength(40)
                .IsRequired();
            Property(p => p.QuantityPerUnit)
                .HasMaxLength(20)
                .IsOptional();
            Property(p => p.UnitPrice)
                .IsOptional();
            Property(p => p.UnitsInStock)
                .IsOptional();
            Property(p => p.UnitsOnOrder)
                .IsOptional();
            Property(p => p.ReorderLevel)
                .IsOptional();
            Property(p => p.Discontinued)
                .IsRequired();
        }
    }
}