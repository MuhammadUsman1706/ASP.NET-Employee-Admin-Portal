using ASP.NET_EMPLOYEE_ADMIN_PORTAL.Data;
using ASP.NET_EMPLOYEE_ADMIN_PORTAL.Features.Employees.Entities;
using ASP.NET_EMPLOYEE_ADMIN_PORTAL.Features.Offices.Entities;
using Microsoft.EntityFrameworkCore;

namespace ASP.NET_EMPLOYEE_ADMIN_PORTAL.Features.Employees
{
    public class EmployeesService
    // : IEmployeesService
    {
        private readonly ApplicationDbContext _dbContext;

        public EmployeesService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Employee>> GetAllEmployeesAsync(int pageNo, int? pageSize, string? sortField, string? sortOrder, string? search)
        {
            int size = pageSize ?? 10;
            string field = sortField ?? "Name";
            string order = string.IsNullOrEmpty(sortOrder) || sortOrder.ToLower() == "ascending" || sortOrder.ToLower() == "asc" ? "asc" : "desc";

            if (pageNo <= 0)
                throw new ArgumentException("Page number must be greater than 0");

            int toSkip = (pageNo - 1) * size;

            // Sort expression
            string sortExpression = $"{field} {order}";

            try
            {
                var employees = await _dbContext.Employees
                    .Select(employee => new Employee
                    {
                        Id = employee.Id,
                        Name = employee.Name,
                        Email = employee.Email,
                        Phone = employee.Phone,
                        Salary = employee.Salary,
                        OfficeId = employee.OfficeId,
                        Office = new Office
                        {
                            Id = employee.Office.Id,
                            Name = employee.Office.Name,
                            Address = employee.Office.Address,
                        }
                    })
                    .Where(e => string.IsNullOrEmpty(search) || e.Name.Contains(search) || e.Email.Contains(search) || e.Phone.Contains(search))
                    .OrderBy(sortExpression)
                    .Skip(toSkip)
                    .Take(size)
                    .ToListAsync();

                return employees;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error fetching employees: {ex.Message}");
            }
        }

    }
}
