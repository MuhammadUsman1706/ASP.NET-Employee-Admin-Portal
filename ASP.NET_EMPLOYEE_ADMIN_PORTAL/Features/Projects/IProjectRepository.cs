using ASP.NET_EMPLOYEE_ADMIN_PORTAL.Features.Projects.Entities;

namespace ASP.NET_EMPLOYEE_ADMIN_PORTAL.Features.Projects
{
    public interface IProjectRepository
    {
        Task<IEnumerable<Project>> GetAllProjects(int pageNo, int pageSize, int toSkip, string sortExpression, string? search);

        Task<Project?> GetProjectById(Guid id);

        Task<Project> AddProject(Project projectEntity);

        Task<Project> UpdateProject(Project projectEntity);

        Task<Project> DeleteProject(Project projectEntity);

        Task<Project> MapProjectToEmployee(Guid projectId, Guid employeeId);
    }
}
