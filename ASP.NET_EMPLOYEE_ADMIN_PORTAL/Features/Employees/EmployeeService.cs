using ASP.NET_EMPLOYEE_ADMIN_PORTAL.Data;
using ASP.NET_EMPLOYEE_ADMIN_PORTAL.Exceptions;
using ASP.NET_EMPLOYEE_ADMIN_PORTAL.Features.Offices;
using ASP.NET_EMPLOYEE_ADMIN_PORTAL.Features.Employees.Dtos;
using ASP.NET_EMPLOYEE_ADMIN_PORTAL.Features.Employees.Entities;
using FluentValidation;


namespace ASP.NET_EMPLOYEE_ADMIN_PORTAL.Features.Employees
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IOfficeRepository _officeRepository;
        private readonly IValidator<Employee> _employeeValidator;

        public EmployeeService(ApplicationDbContext dbContext, IEmployeeRepository employeeRepository, IOfficeRepository officeRepository, IValidator<Employee> employeeValidator)
        {
            _employeeRepository = employeeRepository;
            _officeRepository = officeRepository;
            _employeeValidator = employeeValidator;
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

            return await _employeeRepository.GetAllEmployees(pageNo, size, toSkip, sortExpression, search);
        }

        public async Task<Employee> GetEmployeeByIdAsync(Guid id)
        {
            var employee = await _employeeRepository.GetEmployeeById(id);

            if (employee is null)
                throw new EntityNotFoundException("Employee not found!");

            return employee;
        }

        public async Task<AddEmployeeDto> AddEmployeeAsync(AddEmployeeDto addEmployeeDto)
        {
            var office = await _officeRepository.GetOfficeByIdAsync(addEmployeeDto.OfficeId);

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

            var validationResult = await _employeeValidator.ValidateAsync(employeeEntity);

            if (!validationResult.IsValid)
            {
                var errors = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
                throw new ValidationException($"Employee validation failed: {errors}");
            }

            await _employeeRepository.AddEmployee(employeeEntity);

            return addEmployeeDto;
        }

        public async Task<UpdateEmployeeDto> UpdateEmployeeAsync(Guid id, UpdateEmployeeDto updateEmployeeDto)
        {
            var employee = await _employeeRepository.GetEmployeeById(id);

            if (employee is null)
                throw new EntityNotFoundException("Employee not found!");

            employee.Name = updateEmployeeDto.Name;
            employee.Email = updateEmployeeDto.Email;
            employee.Phone = updateEmployeeDto.Phone;
            employee.Salary = updateEmployeeDto.Salary;

            if (updateEmployeeDto.OfficeId.HasValue)
            {
                var office = await _officeRepository.GetOfficeByIdAsync(updateEmployeeDto.OfficeId.Value);

                if (office is null)
                    throw new EntityNotFoundException("Office not found!");

                employee.OfficeId = updateEmployeeDto.OfficeId.Value;
            }

            var validationResult = await _employeeValidator.ValidateAsync(employee);

            if (!validationResult.IsValid)
            {
                var errors = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
                throw new ValidationException($"Employee validation failed: {errors}");
            }

            await _employeeRepository.UpdateEmployee(employee);

            return updateEmployeeDto;
        }

        public async Task<Employee> DeleteEmployeeAsync(Guid id)
        {
            var employee = await _employeeRepository.GetEmployeeById(id);

            if (employee is null)
                throw new EntityNotFoundException("Employee not found!");

            await _employeeRepository.DeleteEmployee(employee);

            return employee;
        }
    }
}
