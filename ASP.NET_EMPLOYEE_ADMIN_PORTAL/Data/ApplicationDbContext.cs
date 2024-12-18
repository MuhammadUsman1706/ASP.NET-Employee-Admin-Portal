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
    }
}

// shortcuts
// ctor
// prop
// ctrl + .