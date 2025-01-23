using ASP.NET_EMPLOYEE_ADMIN_PORTAL.Data;
using ASP.NET_EMPLOYEE_ADMIN_PORTAL.Features.Offices.Entities;
using ASP.NET_EMPLOYEE_ADMIN_PORTAL.Features.Employees.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;

namespace ASP.NET_EMPLOYEE_ADMIN_PORTAL.Features.Offices
{
    public class OfficeRepository : IOfficeRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public OfficeRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Office>> GetAllOfficesAsync(int pageSize, int toSkip, string sortExpression, string? search)
        {
            var query = _dbContext.Offices
                .Select(o => new Office
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
                    (o.Address != null && o.Address.Contains(search)));
            }

            query = query.OrderBy(sortExpression);

            return await query.Skip(toSkip).Take(pageSize).ToListAsync();
        }

        public async Task<Office?> GetOfficeByIdAsync(Guid id)
        {
            return await _dbContext.Offices.Select(o => new Office
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
            }).FirstOrDefaultAsync(o => o.Id == id);
        }

        public async Task<Office> AddOfficeAsync(Office officeEntity)
        {
            await _dbContext.Offices.AddAsync(officeEntity);
            await _dbContext.SaveChangesAsync();
            return officeEntity;
        }

        public async Task<Office> UpdateOfficeAsync(Office officeEntity)
        {
            _dbContext.Offices.Update(officeEntity);
            await _dbContext.SaveChangesAsync();
            return officeEntity;
        }

        public async Task DeleteOfficeAsync(Office officeEntity)
        {
            _dbContext.Offices.Remove(officeEntity);
            await _dbContext.SaveChangesAsync();
        }
    }
}
