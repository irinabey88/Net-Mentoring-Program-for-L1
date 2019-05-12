using System.Data.Entity.ModelConfiguration;
using Common.Models;

namespace Dataccess.Configuration
{
    public class EmployeeConfiguration: EntityTypeConfiguration<Employee>
    {
        public EmployeeConfiguration()
        {
            ToTable("Employees");
            HasKey(p => p.EmployeeID);
            HasMany(e => e.Territories)
                .WithMany(e => e.Employees)
                .Map(m => m.ToTable("EmployeeTerritories")
                .MapLeftKey("EmployeeID")
                .MapRightKey("TerritoryID"));
            HasMany(e => e.EmployeesTo)
                .WithOptional(e => e.EmployeeTo)
                .HasForeignKey(e => e.ReportsTo);

            Property(p => p.EmployeeID)
                .HasColumnName("EmployeeID");
            Property(p => p.LastName)
                .HasMaxLength(20)
                .IsRequired();
            Property(p => p.FirstName)
                .HasMaxLength(10)
                .IsRequired();
            Property(p => p.Title)
                .HasMaxLength(30)
                .IsOptional();
            Property(p => p.TitleOfCourtesy)
                .HasMaxLength(25)
                .IsOptional();
            Property(p => p.BirthDate)
                .IsOptional();
            Property(p => p.HireDate)
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
            Property(p => p.HomePhone)
                .HasMaxLength(24)
                .IsOptional();
            Property(p => p.Extension)
                .HasMaxLength(4)
                .IsOptional();
            Property(p => p.Photo)
                .HasColumnType("image")
                .IsOptional();
            Property(p => p.Notes)
                .HasColumnType("ntext")
                .IsOptional();
            Property(p => p.PhotoPath)
                .HasMaxLength(224)
                .IsOptional();
            }
    }
}