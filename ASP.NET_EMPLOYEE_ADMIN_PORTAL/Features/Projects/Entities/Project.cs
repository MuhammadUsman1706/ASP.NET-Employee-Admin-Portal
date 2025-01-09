using ASP.NET_EMPLOYEE_ADMIN_PORTAL.Features.Employees.Entities;

namespace ASP.NET_EMPLOYEE_ADMIN_PORTAL.Features.Projects.Entities
{
    public class Project
    {
        public Guid Id { get; set; }

        public required string Name { get; set; }

        public string? Description { get; set; }

        //public List<EmployeeProject> EmployeeProjects { get; } = [];

        public List<Employee> Employees { get; set; } = [];
    }
}
