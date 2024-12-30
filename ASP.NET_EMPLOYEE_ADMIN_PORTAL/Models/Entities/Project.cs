namespace ASP.NET_EMPLOYEE_ADMIN_PORTAL.Models.Entities
{
    public class Project
    {
        public Guid Id { get; set; }

        public required string Name { get; set; }

        public required string Description { get; set; }

        public ICollection<Employee> Employees { get; set; } = new List<Employee>();
    }
}
