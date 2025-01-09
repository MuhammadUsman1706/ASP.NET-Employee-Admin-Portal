using ASP.NET_EMPLOYEE_ADMIN_PORTAL.Features.Employees.Entities;

namespace ASP.NET_EMPLOYEE_ADMIN_PORTAL.Features.Employees
{
    public interface IEmployeesService
    {
        Task<IEnumerable<Employee>> GetAllEmployeesAsync();

        Task<Employee?> GetEmployeeByIdAsync(Guid id);

        Task<Employee> CreateEmployeeAsync(Employee employee);

        Task<bool> DeleteEmployeeAsync(Guid id);
    }
}
