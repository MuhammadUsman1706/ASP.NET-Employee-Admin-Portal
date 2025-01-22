using ASP.NET_EMPLOYEE_ADMIN_PORTAL.Features.Offices.Dtos;
using ASP.NET_EMPLOYEE_ADMIN_PORTAL.Features.Offices.Entities;

namespace ASP.NET_EMPLOYEE_ADMIN_PORTAL.Features.Offices
{
    public interface IOfficeService
    {
        Task<IEnumerable<Office>> GetAllOfficesAsync(int pageNo, int? pageSize, string? sortField, string? sortOrder, string? search);

        Task<Office> GetOfficeByIdAsync(Guid id);

        Task<Office> AddOfficeAsync(AddOfficeDto addOfficeDto);

        Task<Office> UpdateOfficeAsync(Guid id, UpdateOfficeDto updateOfficeDto);

        Task<Office> DeleteOfficeAsync(Guid id);
    }
}
