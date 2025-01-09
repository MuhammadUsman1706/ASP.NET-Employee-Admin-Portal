using ASP.NET_EMPLOYEE_ADMIN_PORTAL.Data;
using ASP.NET_EMPLOYEE_ADMIN_PORTAL.Features.Employees.Entities;
using ASP.NET_EMPLOYEE_ADMIN_PORTAL.Features.Offices.Dtos;
using ASP.NET_EMPLOYEE_ADMIN_PORTAL.Features.Offices.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ASP.NET_EMPLOYEE_ADMIN_PORTAL.Features.Offices
{
    [Route("api/[controller]")]
    [ApiController]
    public class OfficesController : ControllerBase
    {
        private readonly ApplicationDbContext dbContext;

        public OfficesController(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet]
        public IActionResult GetAllOffices(
            [FromQuery(Name = "pageNo")] int pageNo,
            [FromQuery(Name = "pageSize")] int? pageSize,
            [FromQuery(Name = "sortField")] string? sortField,
            [FromQuery(Name = "sortOrder")] string? sortOrder,
            [FromQuery(Name = "search")] string? search)
        {
            if (pageNo <= 0)
                return BadRequest("Page number must be greater than 0");

            int size = pageSize ?? 10;
            string field = sortField ?? "Name";
            bool isAscending = string.IsNullOrEmpty(sortOrder) || sortOrder.ToLower() == "asc" || sortOrder.ToLower() == "ascending";

            int toSkip = (pageNo - 1) * size;

            // Query offices directly from the database
            IQueryable<Office> query = dbContext.Offices.Select(o => new Office
            {
                Id = o.Id,
                Name = o.Name,
                Address = o.Address,
                Employees = o.Employees.Select(e => new Employee
                {
                    Id = e.Id,
                    Name = e.Name,
                    Email = e.Email,
                    Phone = e.Phone,
                    Salary = e.Salary
                }).ToList()
            });

            // Apply search filtering
            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(o =>
                    o.Name.Contains(search) ||
                    o.Address != null && o.Address.Contains(search));
            }

            // Dynamic sorting
            query = isAscending
                ? query.OrderBy(o => EF.Property<object>(o, field))
                : query.OrderByDescending(o => EF.Property<object>(o, field));

            // Pagination
            var allOffices = query
                .Skip(toSkip)
                .Take(size)
                .ToList();

            return Ok(allOffices);
        }


        [HttpGet]
        [Route("{id:guid}")]
        public IActionResult GetOffice(Guid id)
        {
            var office = dbContext.Offices.Select(office => new Office()
            {
                Id = office.Id,
                Name = office.Name,
                Address = office.Address,
                Employees = office.Employees.Select(employee => new Employee()
                {
                    Id = employee.Id,
                    Name = employee.Name,
                    Email = employee.Email,
                    Phone = employee.Phone,
                    Salary = employee.Salary,
                }).ToList()
            }).FirstOrDefault(office => office.Id == id);

            if (office is null)
                return NotFound("Office not found!");

            return Ok(office);
        }

        [HttpPost]
        public IActionResult AddOffice(AddOfficeDto addOfficeDto)
        {
            var officeEntity = new Office()
            {
                Name = addOfficeDto.Name,
                Address = addOfficeDto.Address,
            };

            dbContext.Offices.Add(officeEntity);
            dbContext.SaveChanges();

            return Ok(officeEntity);
        }

        [HttpPut]
        [Route("{id:guid}")]
        public IActionResult UpdateOffice(Guid id, UpdateOfficeDto updateOfficeDto)
        {
            var office = dbContext.Offices.Find(id);

            if (office is null)
                return NotFound("Office not found!");

            office.Name = updateOfficeDto.Name;
            office.Address = updateOfficeDto.Address;

            dbContext.SaveChanges();

            return Ok(office);
        }

        [HttpDelete]
        [Route("{id:guid}")]
        public IActionResult DeleteOffice(Guid id)
        {
            var office = dbContext.Offices.Find(id);

            if (office is null)
                return NotFound("Office not found!");

            dbContext.Offices.Remove(office);
            dbContext.SaveChanges();

            return Ok(office);
        }
    }
}
