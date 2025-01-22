using ASP.NET_EMPLOYEE_ADMIN_PORTAL.Data;
using ASP.NET_EMPLOYEE_ADMIN_PORTAL.Features.Employees.Entities;
using ASP.NET_EMPLOYEE_ADMIN_PORTAL.Features.Offices.Entities;
using ASP.NET_EMPLOYEE_ADMIN_PORTAL.Features.Projects.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;

namespace ASP.NET_EMPLOYEE_ADMIN_PORTAL.Features.Employees
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public EmployeeRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Employee>> GetAllEmployees(int pageNo, int pageSize, int toSkip, string sortExpression, string? search)
        {
            try
            {
                var employees = await _dbContext.Employees
                    .Select(employee => new Employee()
                    {
                        Id = employee.Id,
                        Name = employee.Name,
                        Email = employee.Email,
                        Phone = employee.Phone,
                        Salary = employee.Salary,
                        OfficeId = employee.OfficeId,
                        Office = new Office()
                        {
                            Id = employee.Office.Id,
                            Name = employee.Office.Name,
                            Address = employee.Office.Address,
                        }
                    })
                    .Where(e => string.IsNullOrEmpty(search) || e.Name.Contains(search) || e.Email.Contains(search) || e.Phone.Contains(search))
                    .OrderBy(sortExpression)
                    .Skip(toSkip)
                    .Take(pageSize)
                    .ToListAsync();

                return employees;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error fetching employees: {ex.Message}");
            }
        }

        public async Task<Employee?> GetEmployeeById(Guid id)
        {
            return await _dbContext.Employees.Select(employee => new Employee()
            {
                Id = employee.Id,
                Name = employee.Name,
                Email = employee.Email,
                Phone = employee.Phone,
                Salary = employee.Salary,
                OfficeId = employee.OfficeId,
                Office = new Office()
                {
                    Id = employee.Office.Id,
                    Name = employee.Office.Name,
                    Address = employee.Office.Address,
                },
                Projects = employee.Projects.Select(p => new Project()
                {
                    Name = p.Name,
                    Description = p.Description,
                }).ToList()
            }).FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<Employee> AddEmployee(Employee employeeEntity)
        {
            var employee = await _dbContext.Employees.AddAsync(employeeEntity);

            await _dbContext.SaveChangesAsync();

            return employee.Entity;
        }

        public async Task<Employee> UpdateEmployee(Employee employeeEntity)
        {
            _dbContext.Update(employeeEntity);

            await _dbContext.SaveChangesAsync();

            return employeeEntity;
        }

        public async Task<Employee> DeleteEmployee(Employee employeeEntity)
        {
            _dbContext.Employees.Remove(employeeEntity);

            await _dbContext.SaveChangesAsync();

            return employeeEntity;
        }
    }
}
