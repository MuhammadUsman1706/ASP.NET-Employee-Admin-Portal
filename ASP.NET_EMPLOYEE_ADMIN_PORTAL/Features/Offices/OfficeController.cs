using ASP.NET_EMPLOYEE_ADMIN_PORTAL.Features.Offices.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace ASP.NET_EMPLOYEE_ADMIN_PORTAL.Features.Offices
{
    [Route("api/[controller]")]
    [ApiController]
    public class OfficeController : ControllerBase
    {
        private readonly IOfficeService _officesService;

        public OfficeController(IOfficeService officesService)
        {
            _officesService = officesService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllOffices(
            [FromQuery(Name = "pageNo")] int pageNo,
            [FromQuery(Name = "pageSize")] int? pageSize,
            [FromQuery(Name = "sortField")] string? sortField,
            [FromQuery(Name = "sortOrder")] string? sortOrder,
            [FromQuery(Name = "search")] string? search)
        {
            var offices = await _officesService.GetAllOfficesAsync(pageNo, pageSize, sortField, sortOrder, search);

            return Ok(offices);
        }

        [HttpGet]
        [Route("{id:guid}")]
        public async Task<IActionResult> GetOfficeById(Guid id)
        {
            var office = await _officesService.GetOfficeByIdAsync(id);

            return Ok(office);
        }

        [HttpPost]
        public async Task<IActionResult> AddOffice(AddOfficeDto addOfficeDto)
        {
            var office = await _officesService.AddOfficeAsync(addOfficeDto);

            return Ok(office);
        }

        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> UpdateOffice(Guid id, UpdateOfficeDto updateOfficeDto)
        {
            var office = await _officesService.UpdateOfficeAsync(id, updateOfficeDto);

            return Ok(office);
        }

        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> DeleteOffice(Guid id)
        {
            var office = await _officesService.DeleteOfficeAsync(id);

            return Ok(office);
        }
    }
}
