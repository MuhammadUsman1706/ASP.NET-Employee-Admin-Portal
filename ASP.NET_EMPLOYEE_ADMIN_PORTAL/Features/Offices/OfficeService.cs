using ASP.NET_EMPLOYEE_ADMIN_PORTAL.Exceptions;
using ASP.NET_EMPLOYEE_ADMIN_PORTAL.Features.Offices.Dtos;
using ASP.NET_EMPLOYEE_ADMIN_PORTAL.Features.Offices.Entities;

namespace ASP.NET_EMPLOYEE_ADMIN_PORTAL.Features.Offices
{
    public class OfficeService : IOfficeService
    {
        private readonly IOfficeRepository _officeRepository;

        public OfficeService(IOfficeRepository officeRepository)
        {
            _officeRepository = officeRepository;
        }

        public async Task<IEnumerable<Office>> GetAllOfficesAsync(int pageNo, int? pageSize, string? sortField, string? sortOrder, string? search)
        {
            if (pageNo <= 0)
                throw new ArgumentException("Page number must be greater than 0");

            int size = pageSize ?? 10;
            string field = sortField ?? "Name";
            string order = string.IsNullOrEmpty(sortOrder) || sortOrder.ToLower() == "ascending" || sortOrder.ToLower() == "asc" ? "asc" : "desc";
            int toSkip = (pageNo - 1) * size;

            string sortExpression = $"{field} {order}";

            return await _officeRepository.GetAllOfficesAsync(size, toSkip, sortExpression, search);
        }

        public async Task<Office> GetOfficeByIdAsync(Guid id)
        {
            var office = await _officeRepository.GetOfficeByIdAsync(id);

            if (office is null)
                throw new EntityNotFoundException("Office not found!");

            return office;
        }

        public async Task<Office> AddOfficeAsync(AddOfficeDto addOfficeDto)
        {
            var officeEntity = new Office
            {
                Name = addOfficeDto.Name,
                Address = addOfficeDto.Address
            };

            return await _officeRepository.AddOfficeAsync(officeEntity);
        }

        public async Task<Office> UpdateOfficeAsync(Guid id, UpdateOfficeDto updateOfficeDto)
        {
            var office = await _officeRepository.GetOfficeByIdAsync(id);

            if (office is null)
                throw new EntityNotFoundException("Office not found!");

            office.Name = updateOfficeDto.Name;
            office.Address = updateOfficeDto.Address;

            return await _officeRepository.UpdateOfficeAsync(office);
        }

        public async Task<Office> DeleteOfficeAsync(Guid id)
        {
            var office = await _officeRepository.GetOfficeByIdAsync(id);

            if (office is null)
                throw new EntityNotFoundException("Office not found!");

            await _officeRepository.DeleteOfficeAsync(office);

            return office;
        }
    }
}
