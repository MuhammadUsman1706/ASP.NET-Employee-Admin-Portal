using ASP.NET_EMPLOYEE_ADMIN_PORTAL.Data;
using ASP.NET_EMPLOYEE_ADMIN_PORTAL.Models;
using ASP.NET_EMPLOYEE_ADMIN_PORTAL.Models.Entities;
using Microsoft.AspNetCore.Mvc;

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
        public IActionResult GetAllEmployees()
        {
            var allEmployees = dbContext.Employees.ToList<Employee>();

            return Ok(allEmployees);
        }

        [HttpGet]
        [Route("{id:guid}")]
        public IActionResult GetEmployeeById(Guid id)
        {
            var employee = dbContext.Employees.Find(id);

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

            return Ok(employeeEntity);
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
                var office = dbContext.Offices.Find(id);

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
