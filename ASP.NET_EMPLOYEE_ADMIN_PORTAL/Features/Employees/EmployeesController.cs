using ASP.NET_EMPLOYEE_ADMIN_PORTAL.Features.Employees.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace ASP.NET_EMPLOYEE_ADMIN_PORTAL.Features.Employees
{
    // localhost:xxxx/api/employees
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeesController : ControllerBase
    {
        //private readonly ApplicationDbContext dbContext;
        private readonly IEmployeesService _employeesService;

        public EmployeesController(IEmployeesService employeesService)
        {
            //this.dbContext = dbContext;
            this._employeesService = employeesService;
        }


        [HttpGet]
        public async Task<IActionResult> GetAllEmployees(
            [FromQuery(Name = "pageNo")] int pageNo,
            [FromQuery(Name = "pageSize")] int? pageSize,
            [FromQuery(Name = "sortField")] string? sortField,
            [FromQuery(Name = "sortOrder")] string? sortOrder,
            [FromQuery(Name = "search")] string? search)
        {
            var employees = await _employeesService.GetAllEmployeesAsync(pageNo, pageSize, sortField, sortOrder, search);

            return Ok(employees);
        }

        [HttpGet]
        [Route("{id:guid}")]
        public async Task<IActionResult> GetEmployeeById(Guid id)
        {
            var employee = await _employeesService.GetEmployeeByIdAsync(id);

            return Ok(employee);
        }

        [HttpPost]
        public async Task<IActionResult> AddEmployee(AddEmployeeDto addEmployeeDto)
        {
            var employee = await _employeesService.AddEmployeeAsync(addEmployeeDto);

            return Ok(employee);
        }

        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> UpdateEmployee(Guid id, UpdateEmployeeDto updateEmployeeDto)
        {
            var employee = await _employeesService.UpdateEmployeeAsync(id, updateEmployeeDto);

            return Ok(employee);
        }

        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> DeleteEmployee(Guid id)
        {
            var employee = await _employeesService.DeleteEmployeeAsync(id);

            return Ok(employee);
        }

    }
}
