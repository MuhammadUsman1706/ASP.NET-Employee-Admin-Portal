using ASP.NET_EMPLOYEE_ADMIN_PORTAL.Data;
using ASP.NET_EMPLOYEE_ADMIN_PORTAL.Exceptions;
using ASP.NET_EMPLOYEE_ADMIN_PORTAL.Features.Employees.Entities;
using ASP.NET_EMPLOYEE_ADMIN_PORTAL.Features.Projects.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;

namespace ASP.NET_EMPLOYEE_ADMIN_PORTAL.Features.Projects
{
    public class ProjectRepository : IProjectRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public ProjectRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Project>> GetAllProjects(int pageNo, int pageSize, int toSkip, string sortExpression, string? search)
        {
            var projects = await _dbContext.Projects
                .Select(project => new Project
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
                })
                .Where(p => string.IsNullOrEmpty(search) || p.Name.Contains(search) || p.Description.Contains(search))
                .OrderBy(sortExpression)
                .Skip(toSkip)
                .Take(pageSize)
                .ToListAsync();

            return projects;
        }

        public async Task<Project?> GetProjectById(Guid id)
        {
            return await _dbContext.Projects
                .Include(p => p.Employees)
                .Select(project => new Project
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
                })
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Project> AddProject(Project projectEntity)
        {
            var project = await _dbContext.Projects.AddAsync(projectEntity);
            await _dbContext.SaveChangesAsync();
            return project.Entity;
        }

        public async Task<Project> UpdateProject(Project projectEntity)
        {
            _dbContext.Projects.Update(projectEntity);
            await _dbContext.SaveChangesAsync();
            return projectEntity;
        }

        public async Task<Project> DeleteProject(Project projectEntity)
        {
            _dbContext.Projects.Remove(projectEntity);
            await _dbContext.SaveChangesAsync();
            return projectEntity;
        }

        public async Task<Project> MapProjectToEmployee(Guid projectId, Guid employeeId)
        {
            var project = await _dbContext.Projects.Include(p => p.Employees).FirstOrDefaultAsync(p => p.Id == projectId);

            if (project == null)
                throw new EntityNotFoundException("Project not found!");

            var employee = await _dbContext.Employees.FirstOrDefaultAsync(e => e.Id == employeeId);

            if (employee == null)
                throw new EntityNotFoundException("Employee not found!");

            project.Employees.Add(employee);
            await _dbContext.SaveChangesAsync();
            return project;
        }
    }
}
