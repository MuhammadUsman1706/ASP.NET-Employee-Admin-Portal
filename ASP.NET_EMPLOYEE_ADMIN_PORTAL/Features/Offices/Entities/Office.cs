using ASP.NET_EMPLOYEE_ADMIN_PORTAL.Features.Employees.Entities;

namespace ASP.NET_EMPLOYEE_ADMIN_PORTAL.Features.Offices.Entities
{
    public class Office
    {
        public Guid Id { get; set; }

        public required string Name { get; set; }

        public string? Address { get; set; }

        public ICollection<Employee> Employees { get; set; } = new List<Employee>();
    }
}
