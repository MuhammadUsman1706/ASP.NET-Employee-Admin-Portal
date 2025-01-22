using ASP.NET_EMPLOYEE_ADMIN_PORTAL.Features.Employees.Entities;

namespace ASP.NET_EMPLOYEE_ADMIN_PORTAL.Features.Employees
{
    public interface IEmployeeRepository
    {
        Task<IEnumerable<Employee>> GetAllEmployees(int pageNo, int pageSize, int toSkip, string sortExpression, string? search);

        Task<Employee?> GetEmployeeById(Guid employeeId);

        Task<Employee> AddEmployee(Employee employeeEntity);

        Task<Employee> UpdateEmployee(Employee employeeEntity);

        Task<Employee> DeleteEmployee(Employee employeeEntity);
    }
}
