﻿namespace ASP.NET_EMPLOYEE_ADMIN_PORTAL.Models.Entities
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
        public ICollection<Project> Projects { get; set; } = new List<Project>();
    }
}
