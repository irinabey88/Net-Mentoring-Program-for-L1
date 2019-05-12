using System.Data.Entity;
using Common.Models;
using Dataccess.Configuration;

namespace Dataccess
{
    public class NorthwindContext : DbContext
    {
        public NorthwindContext() 
            : base("name=NorthwindConection")
        {
        }

        public virtual IDbSet<Customer> Customers { get; set; }

        public virtual IDbSet<Employee> Employees { get; set; }

        public virtual IDbSet<Order> Orders { get; set; }

        public virtual IDbSet<Category> Categories { get; set; }

        public virtual IDbSet<Shipper> Shippers { get; set; }

        public virtual IDbSet<Supplier> Suppliers { get; set; }

        public virtual IDbSet<CustomerDemographic> CustomerDemographics { get; set; }

        public virtual IDbSet<OrderDetail> OrderDetails { get; set; }

        public virtual IDbSet<Product> Products { get; set; }

        public virtual IDbSet<Region> Regions { get; set; }

        public virtual IDbSet<Territory> Territories { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Configurations.Add(new CategoryConfiguration());

            modelBuilder.Configurations.Add(new CustomerConfiguration());
            modelBuilder.Configurations.Add(new CustomerDemographicConfiguration());
            modelBuilder.Configurations.Add(new EmployeeConfiguration());
            modelBuilder.Configurations.Add(new OrderConfiguration());
            modelBuilder.Configurations.Add(new OrderDetailConfiguration());
            modelBuilder.Configurations.Add(new ProductConfiguration());
            modelBuilder.Configurations.Add(new RegionConfiguration());
            modelBuilder.Configurations.Add(new ShipperConfiguration());
            modelBuilder.Configurations.Add(new SupplierConfiguration());
            modelBuilder.Configurations.Add(new TerritoryConfiguration());
        }
    }
}