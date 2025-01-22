using ASP.NET_EMPLOYEE_ADMIN_PORTAL.Features.Employees.Dtos;
using ASP.NET_EMPLOYEE_ADMIN_PORTAL.Features.Employees.Entities;

namespace ASP.NET_EMPLOYEE_ADMIN_PORTAL.Features.Employees
{
    public interface IEmployeeService
    {
        Task<IEnumerable<Employee>> GetAllEmployeesAsync(int pageNo, int? pageSize, string? sortField, string? sortOrder, string? search);

        Task<Employee> GetEmployeeByIdAsync(Guid id);

        Task<AddEmployeeDto> AddEmployeeAsync(AddEmployeeDto addEmployeeDto);

        Task<UpdateEmployeeDto> UpdateEmployeeAsync(Guid id, UpdateEmployeeDto updateEmployeeDto);

        Task<Employee> DeleteEmployeeAsync(Guid id);
    }
}
