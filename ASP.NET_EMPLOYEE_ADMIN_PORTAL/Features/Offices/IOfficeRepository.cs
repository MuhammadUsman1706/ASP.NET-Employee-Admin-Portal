using ASP.NET_EMPLOYEE_ADMIN_PORTAL.Features.Offices.Entities;

namespace ASP.NET_EMPLOYEE_ADMIN_PORTAL.Features.Offices
{
    public interface IOfficeRepository
    {
        Task<IEnumerable<Office>> GetAllOfficesAsync(int pageSize, int toSkip, string sortExpression, string? search);

        Task<Office?> GetOfficeByIdAsync(Guid id);

        Task<Office> AddOfficeAsync(Office officeEntity);

        Task<Office> UpdateOfficeAsync(Office officeEntity);

        Task DeleteOfficeAsync(Office officeEntity);
    }
}