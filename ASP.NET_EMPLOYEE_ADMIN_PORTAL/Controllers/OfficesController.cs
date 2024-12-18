using ASP.NET_EMPLOYEE_ADMIN_PORTAL.Data;
using ASP.NET_EMPLOYEE_ADMIN_PORTAL.Models;
using ASP.NET_EMPLOYEE_ADMIN_PORTAL.Models.Entities;
using Microsoft.AspNetCore.Mvc;

namespace ASP.NET_EMPLOYEE_ADMIN_PORTAL.Controllers
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
        public IActionResult GetAllOffices()
        {
            var allOffices = dbContext.Offices.ToList<Office>();

            return Ok(allOffices);
        }

        [HttpGet]
        [Route("{id:guid}")]
        public IActionResult GetOffice(Guid id)
        {
            var office = dbContext.Offices.Find(id);

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
