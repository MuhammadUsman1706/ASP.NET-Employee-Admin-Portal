using ASP.NET_EMPLOYEE_ADMIN_PORTAL.Features.Employees.Entities;
using FluentValidation;

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

    public class ProjectValidator : AbstractValidator<Project>
    {
        public ProjectValidator()
        {
            RuleFor(x => x.Id).NotNull();

            RuleFor(x => x.Name).Length(0, 50);
        }
    }
}
