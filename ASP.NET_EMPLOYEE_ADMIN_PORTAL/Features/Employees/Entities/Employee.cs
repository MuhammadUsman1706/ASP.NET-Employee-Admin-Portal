using ASP.NET_EMPLOYEE_ADMIN_PORTAL.Features.Offices.Entities;
using ASP.NET_EMPLOYEE_ADMIN_PORTAL.Features.Projects.Entities;
using FluentValidation;

namespace ASP.NET_EMPLOYEE_ADMIN_PORTAL.Features.Employees.Entities
{
    public class Employee
    {
        public Guid Id { get; set; }

        public required string Name { get; set; }

        public required string Email { get; set; }

        public string? Phone { get; set; }

        public decimal Salary { get; set; }

        public Guid OfficeId { get; set; }

        public Office Office { get; set; } = null!;

        //public List<EmployeeProject> EmployeeProjects { get; } = [];

        public List<Project> Projects { get; set; } = [];
    }

    public class EmployeeValidator : AbstractValidator<Employee>
    {
        public EmployeeValidator()
        {
            RuleFor(x => x.Id).NotNull();

            RuleFor(x => x.Name).Length(0, 50);

            RuleFor(x => x.Email).EmailAddress();

            RuleFor(x => x.Phone).Length(12);

            RuleFor(x => x.Salary).InclusiveBetween(0, 1000000);
        }
    }
}
