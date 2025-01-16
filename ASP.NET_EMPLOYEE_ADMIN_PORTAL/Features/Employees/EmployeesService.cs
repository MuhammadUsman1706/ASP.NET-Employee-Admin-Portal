using ASP.NET_EMPLOYEE_ADMIN_PORTAL.Data;
using ASP.NET_EMPLOYEE_ADMIN_PORTAL.Exceptions;
using ASP.NET_EMPLOYEE_ADMIN_PORTAL.Features.Employees.Dtos;
using ASP.NET_EMPLOYEE_ADMIN_PORTAL.Features.Employees.Entities;
using ASP.NET_EMPLOYEE_ADMIN_PORTAL.Features.Offices.Entities;
using ASP.NET_EMPLOYEE_ADMIN_PORTAL.Features.Projects.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;

namespace ASP.NET_EMPLOYEE_ADMIN_PORTAL.Features.Employees
{
    public class EmployeesService : IEmployeesService
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

            // Sort expression: "Name asc" or "Name desc"
            string sortExpression = $"{field} {order}";

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
                    .Take(size)
                    .ToListAsync();

                return employees;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error fetching employees: {ex.Message}");
            }
        }

        public async Task<Employee> GetEmployeeByIdAsync(Guid id)
        {
            var employee = await _dbContext.Employees.Select(employee => new Employee()
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

            if (employee is null)
                throw new EntityNotFoundException("Employee not found!");

            return employee;
        }

        public async Task<AddEmployeeDto> AddEmployeeAsync(AddEmployeeDto addEmployeeDto)
        {
            try
            {
                var office = await _dbContext.Offices.FindAsync(addEmployeeDto.OfficeId);

                if (office is null)
                    throw new EntityNotFoundException("Office not found!");

                var employeeEntity = new Employee()
                {
                    Name = addEmployeeDto.Name,
                    Email = addEmployeeDto.Email,
                    Phone = addEmployeeDto.Phone,
                    Salary = addEmployeeDto.Salary,
                    OfficeId = addEmployeeDto.OfficeId,
                };

                await _dbContext.Employees.AddAsync(employeeEntity);
                await _dbContext.SaveChangesAsync();

                return addEmployeeDto;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error adding employee: {ex.Message}");
            }
        }

        public async Task<UpdateEmployeeDto> UpdateEmployeeAsync(Guid id, UpdateEmployeeDto updateEmployeeDto)
        {
            try
            {
                var employee = await _dbContext.Employees.FindAsync(id);

                if (employee is null)
                    throw new EntityNotFoundException("Employee not found!");

                employee.Name = updateEmployeeDto.Name;
                employee.Email = updateEmployeeDto.Email;
                employee.Phone = updateEmployeeDto.Phone;
                employee.Salary = updateEmployeeDto.Salary;

                if (updateEmployeeDto.OfficeId.HasValue)
                {
                    var office = await _dbContext.Offices.FindAsync(updateEmployeeDto.OfficeId);

                    if (office is null)
                        throw new EntityNotFoundException("Office not found!");

                    employee.OfficeId = updateEmployeeDto.OfficeId.Value;
                }

                await _dbContext.SaveChangesAsync();

                return updateEmployeeDto;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error updating employee: {ex.Message}");
            }
        }

        public async Task<Employee> DeleteEmployeeAsync(Guid id)
        {
            try
            {
                var employee = await _dbContext.Employees.FindAsync(id);

                if (employee is null)
                    throw new EntityNotFoundException("Employee not found!");

                _dbContext.Employees.Remove(employee);
                await _dbContext.SaveChangesAsync();

                return employee;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error deleting employee: {ex.Message}");
            }
        }
    }
}
