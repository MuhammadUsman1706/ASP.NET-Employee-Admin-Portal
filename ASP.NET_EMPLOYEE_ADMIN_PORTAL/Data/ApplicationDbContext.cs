using ASP.NET_EMPLOYEE_ADMIN_PORTAL.Features.Employees.Entities;
using ASP.NET_EMPLOYEE_ADMIN_PORTAL.Features.Offices.Entities;
using ASP.NET_EMPLOYEE_ADMIN_PORTAL.Features.Projects.Entities;
using Microsoft.EntityFrameworkCore;

namespace ASP.NET_EMPLOYEE_ADMIN_PORTAL.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Employee> Employees { get; set; }

        public DbSet<Office> Offices { get; set; }

        public DbSet<Project> Projects { get; set; }

        //public DbSet<EmployeeProject> EmployeeProjects { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Office>()
                .HasMany(o => o.Employees)
                .WithOne(e => e.Office)
                .HasForeignKey(e => e.OfficeId)
                .IsRequired();

            modelBuilder.Entity<Employee>()
                .HasMany(e => e.Projects)
                .WithMany(e => e.Employees);
            //.UsingEntity(
            //    "EmployeeProject",
            //    l => l.HasOne(typeof(Project)).WithMany().HasForeignKey("ProjectId").HasPrincipalKey(nameof(Project.Id)),
            //    r => r.HasOne(typeof(Employee)).WithMany().HasForeignKey("EmployeeId").HasPrincipalKey(nameof(Employee.Id)),
            //    j => j.HasKey("EmployeeId", "ProjectId"));
        }
    }
}

// shortcuts
// ctor
// prop
// ctrl + .