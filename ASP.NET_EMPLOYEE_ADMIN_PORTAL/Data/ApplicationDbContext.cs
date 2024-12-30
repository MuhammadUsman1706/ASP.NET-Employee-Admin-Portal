using ASP.NET_EMPLOYEE_ADMIN_PORTAL.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace ASP.NET_EMPLOYEE_ADMIN_PORTAL.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<Office> Offices { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Office>()
                .HasMany(o => o.Employees)
                .WithOne(e => e.Office)
                .HasForeignKey(e => e.OfficeId)
                .IsRequired();

            //modelBuilder.Entity<Project>()
            //    .HasMany(p => p.Employees)
            //    .WithMany(e => e.Projects)
            //    .UsingEntity("EmployeeProject")
            //    .
        }
    }
}

// shortcuts
// ctor
// prop
// ctrl + .