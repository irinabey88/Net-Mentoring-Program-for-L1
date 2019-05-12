using System.Data.Entity.ModelConfiguration;
using Common.Models;

namespace Dataccess.Configuration
{
    public class OrderDetailConfiguration: EntityTypeConfiguration<OrderDetail>
    {
        public OrderDetailConfiguration()
        {
            ToTable("Order Details");
            HasKey(p => new { p.OrderID, p.ProductID});

            Property(p => p.Quantity).IsRequired();
            Property(p => p.UnitPrice)
                .HasPrecision(19, 4)
                .IsRequired();
            Property(p => p.Discount).IsRequired();

            HasRequired(p => p.Order)
                .WithMany(p => p.OrderDetails);
            HasRequired(p => p.Product);
        }
    }
}