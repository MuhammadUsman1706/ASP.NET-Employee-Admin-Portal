using ASP.NET_EMPLOYEE_ADMIN_PORTAL.Data;
using ASP.NET_EMPLOYEE_ADMIN_PORTAL.Exceptions;
using ASP.NET_EMPLOYEE_ADMIN_PORTAL.Features.Employees.Entities;
using ASP.NET_EMPLOYEE_ADMIN_PORTAL.Features.Offices.Dtos;
using ASP.NET_EMPLOYEE_ADMIN_PORTAL.Features.Offices.Entities;
using Microsoft.EntityFrameworkCore;

namespace ASP.NET_EMPLOYEE_ADMIN_PORTAL.Features.Offices
{
    public class OfficeService : IOfficeService
    {
        private readonly ApplicationDbContext _dbContext;

        public OfficeService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Office>> GetAllOfficesAsync(int pageNo, int? pageSize, string? sortField, string? sortOrder, string? search)
        {
            if (pageNo <= 0)
                throw new ArgumentException("Page number must be greater than 0");

            int size = pageSize ?? 10;
            string field = sortField ?? "Name";
            bool isAscending = string.IsNullOrEmpty(sortOrder) || sortOrder.ToLower() == "asc" || sortOrder.ToLower() == "ascending";

            int toSkip = (pageNo - 1) * size;

            IQueryable<Office> query = _dbContext.Offices.Select(o => new Office
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

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(o =>
                    o.Name.Contains(search) ||
                    o.Address != null && o.Address.Contains(search));
            }

            query = isAscending
                ? query.OrderBy(o => EF.Property<object>(o, field))
                : query.OrderByDescending(o => EF.Property<object>(o, field));

            return await query.Skip(toSkip).Take(size).ToListAsync();
        }

        public async Task<Office> GetOfficeByIdAsync(Guid id)
        {
            var office = await _dbContext.Offices.Select(o => new Office
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
                    Salary = e.Salary,
                }).ToList()
            }).FirstOrDefaultAsync(o => o.Id == id);

            if (office is null)
                throw new EntityNotFoundException("Office not found!");

            return office;
        }

        public async Task<Office> AddOfficeAsync(AddOfficeDto addOfficeDto)
        {
            var officeEntity = new Office
            {
                Name = addOfficeDto.Name,
                Address = addOfficeDto.Address,
            };

            await _dbContext.Offices.AddAsync(officeEntity);
            await _dbContext.SaveChangesAsync();

            return officeEntity;
        }

        public async Task<Office> UpdateOfficeAsync(Guid id, UpdateOfficeDto updateOfficeDto)
        {
            var office = await _dbContext.Offices.FindAsync(id);

            if (office is null)
                throw new EntityNotFoundException("Office not found!");

            office.Name = updateOfficeDto.Name;
            office.Address = updateOfficeDto.Address;

            await _dbContext.SaveChangesAsync();

            return office;
        }

        public async Task<Office> DeleteOfficeAsync(Guid id)
        {
            var office = await _dbContext.Offices.FindAsync(id);

            if (office is null)
                throw new EntityNotFoundException("Office not found!");

            _dbContext.Offices.Remove(office);
            await _dbContext.SaveChangesAsync();

            return office;
        }
    }
}
