namespace ASP.NET_EMPLOYEE_ADMIN_PORTAL.Models.Entities
{
    public class Office
    {
        public Guid Id { get; set; }

        public required string Name { get; set; }

        public string? Address { get; set; }

        public ICollection<Employee> Employees { get; } = new List<Employee>();
    }
}
