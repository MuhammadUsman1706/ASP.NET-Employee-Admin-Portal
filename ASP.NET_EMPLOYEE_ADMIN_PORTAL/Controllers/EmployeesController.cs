using ASP.NET_EMPLOYEE_ADMIN_PORTAL.Data;
using ASP.NET_EMPLOYEE_ADMIN_PORTAL.Models;
using ASP.NET_EMPLOYEE_ADMIN_PORTAL.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;

namespace ASP.NET_EMPLOYEE_ADMIN_PORTAL.Controllers
{
    // localhost:xxxx/api/employees
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeesController : ControllerBase
    {
        private readonly ApplicationDbContext dbContext;

        public EmployeesController(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }


        [HttpGet]
        public IActionResult GetAllEmployees(
            [FromQuery(Name = "pageNo")] int pageNo,
            [FromQuery(Name = "pageSize")] int? pageSize,
            [FromQuery(Name = "sortField")] string? sortField,
            [FromQuery(Name = "sortOrder")] string? sortOrder,
            [FromQuery(Name = "search")] string? search)
        {
            int size = pageSize ?? 10;
            string field = sortField ?? "Name";
            string order = (string.IsNullOrEmpty(sortOrder) || sortOrder.ToLower() == "ascending" || sortOrder.ToLower() == "asc") ? "asc" : "desc";

            if (pageNo <= 0)
                return BadRequest("Page number must be greater than 0");


            int toSkip = (pageNo - 1) * size;

            // Sort expression: "Name asc" or "Name desc"
            string sortExpression = $"{field} {order}";

            try
            {
                var allEmployees = dbContext.Employees
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
                    .Where(e => (search == null || e.Name.Contains(search) || e.Email.Contains(search) || e.Phone.Contains(search)))
                    .OrderBy(sortExpression)
                    .Skip(toSkip)
                    .Take(size)
                    .ToList<Employee>();

                return Ok(allEmployees);
            }
            catch (Exception ex)
            {
                // Handle invalid field names or other errors
                return BadRequest(new { message = $"Invalid sort field or order: {ex.Message}" });
            }
        }

        [HttpGet]
        [Route("{id:guid}")]
        public IActionResult GetEmployeeById(Guid id)
        {
            var employee = dbContext.Employees.Select(employee => new Employee()
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
            }).FirstOrDefault(e => e.Id == id);

            if (employee is null)
                return NotFound("Employee not found!");

            return Ok(employee);
        }

        [HttpPost]
        public IActionResult AddEmployee(AddEmployeeDto addEmployeeDto)
        {
            var office = dbContext.Offices.Find(addEmployeeDto.OfficeId);

            if (office is null)
                return NotFound("Office not found!");

            var employeeEntity = new Employee()
            {
                Name = addEmployeeDto.Name,
                Email = addEmployeeDto.Email,
                Phone = addEmployeeDto.Phone,
                Salary = addEmployeeDto.Salary,
                OfficeId = addEmployeeDto.OfficeId,
            };

            dbContext.Employees.Add(employeeEntity);
            dbContext.SaveChanges();

            return Ok(addEmployeeDto);
        }

        [HttpPut]
        [Route("{id:guid}")]
        public IActionResult UpdateEmployee(Guid id, UpdateEmployeeDto updateEmployeeDto)
        {
            var employee = dbContext.Employees.Find(id);

            if (employee is null)
                return NotFound("Employee not found!");

            employee.Name = updateEmployeeDto.Name;
            employee.Email = updateEmployeeDto.Email;
            employee.Phone = updateEmployeeDto.Phone;
            employee.Salary = updateEmployeeDto.Salary;

            if (updateEmployeeDto.OfficeId.HasValue)
            {
                var office = dbContext.Offices.Find(updateEmployeeDto.OfficeId);

                if (office is null)
                    return NotFound("Office not found!");

                employee.OfficeId = updateEmployeeDto.OfficeId.Value;
            }

            dbContext.SaveChanges();

            return Ok(employee);
        }

        [HttpDelete]
        [Route("{id:guid}")]
        public IActionResult DeleteEmployee(Guid id)
        {
            var employee = dbContext.Employees.Find(id);

            if (employee is null)
                return NotFound("Employee not found!");

            dbContext.Employees.Remove(employee);
            dbContext.SaveChanges();

            return Ok(employee);
        }

    }
}
