namespace ASP.NET_EMPLOYEE_ADMIN_PORTAL.Models
{
    public class UpdateEmployeeDto
    {
        public required string Name { get; set; }

        public required string Email { get; set; }

        public string? Phone { get; set; }

        public decimal Salary { get; set; }

        public Guid? OfficeId { get; set; }
    }
}
