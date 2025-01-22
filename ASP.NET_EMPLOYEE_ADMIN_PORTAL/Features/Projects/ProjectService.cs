using ASP.NET_EMPLOYEE_ADMIN_PORTAL.Data;
using ASP.NET_EMPLOYEE_ADMIN_PORTAL.Exceptions;
using ASP.NET_EMPLOYEE_ADMIN_PORTAL.Features.Employees.Entities;
using ASP.NET_EMPLOYEE_ADMIN_PORTAL.Features.Projects.Dtos;
using ASP.NET_EMPLOYEE_ADMIN_PORTAL.Features.Projects.Entities;
using Microsoft.EntityFrameworkCore;

namespace ASP.NET_EMPLOYEE_ADMIN_PORTAL.Features.Projects
{
    public class ProjectService : IProjectService
    {
        private readonly ApplicationDbContext _dbContext;

        public ProjectService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Project>> GetAllProjectsAsync(int pageNo, int? pageSize, string? sortField, string? sortOrder, string? search)
        {
            if (pageNo <= 0)
                throw new ArgumentException("Page number must be greater than 0");

            int size = pageSize ?? 10;
            string field = sortField ?? "Name";
            bool isAscending = string.IsNullOrEmpty(sortOrder) || sortOrder.ToLower() == "asc" || sortOrder.ToLower() == "ascending";
            int toSkip = (pageNo - 1) * size;

            IQueryable<Project> query = _dbContext.Projects.Select(project => new Project
            {
                Id = project.Id,
                Name = project.Name,
                Description = project.Description,
                Employees = project.Employees.Select(e => new Employee
                {
                    Id = e.Id,
                    Name = e.Name,
                    Email = e.Email,
                    Phone = e.Phone,
                    Salary = e.Salary,
                    OfficeId = e.OfficeId
                }).ToList()
            });

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(p => p.Name.Contains(search) || p.Description.Contains(search));
            }

            query = isAscending
                ? query.OrderBy(p => EF.Property<object>(p, field))
                : query.OrderByDescending(p => EF.Property<object>(p, field));

            return await query.Skip(toSkip).Take(size).ToListAsync();
        }

        public async Task<Project> GetProjectByIdAsync(Guid id)
        {
            var project = await _dbContext.Projects.Select(project => new Project
            {
                Id = project.Id,
                Name = project.Name,
                Description = project.Description,
                Employees = project.Employees.Select(e => new Employee
                {
                    Id = e.Id,
                    Name = e.Name,
                    Email = e.Email,
                    Phone = e.Phone,
                    Salary = e.Salary,
                    OfficeId = e.OfficeId
                }).ToList()
            }).FirstOrDefaultAsync(p => p.Id == id);

            if (project is null)
                throw new EntityNotFoundException("Project not found!");

            return project;
        }

        public async Task<Project> AddProjectAsync(AddProjectDto addProjectDto)
        {
            var projectEntity = new Project
            {
                Name = addProjectDto.Name,
                Description = addProjectDto.Description
            };

            await _dbContext.Projects.AddAsync(projectEntity);
            await _dbContext.SaveChangesAsync();

            return projectEntity;
        }

        public async Task<Project> UpdateProjectAsync(Guid id, UpdateProjectDto updateProjectDto)
        {
            var project = await _dbContext.Projects.FindAsync(id);

            if (project is null)
                throw new EntityNotFoundException("Project not found!");

            project.Name = updateProjectDto.Name;
            project.Description = updateProjectDto.Description;

            await _dbContext.SaveChangesAsync();

            return project;
        }

        public async Task<Project> DeleteProjectAsync(Guid id)
        {
            var project = await _dbContext.Projects.FindAsync(id);

            if (project is null)
                throw new EntityNotFoundException("Project not found!");

            _dbContext.Projects.Remove(project);
            await _dbContext.SaveChangesAsync();

            return project;
        }

        public async Task<Project> MapProjectToEmployeeAsync(MapProjectEmployeeDto mapProjectEmployeeDto)
        {
            var project = await _dbContext.Projects.Include(p => p.Employees).FirstOrDefaultAsync(p => p.Id == mapProjectEmployeeDto.projectId);

            if (project is null)
                throw new EntityNotFoundException("Project not found!");

            var employee = await _dbContext.Employees.FindAsync(mapProjectEmployeeDto.employeeId);

            if (employee is null)
                throw new EntityNotFoundException("Employee not found!");

            project.Employees.Add(employee);
            await _dbContext.SaveChangesAsync();

            return project;
        }
    }
}