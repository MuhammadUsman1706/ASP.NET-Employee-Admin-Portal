using ASP.NET_EMPLOYEE_ADMIN_PORTAL.Features.Projects.Dtos;
using ASP.NET_EMPLOYEE_ADMIN_PORTAL.Features.Projects.Entities;

namespace ASP.NET_EMPLOYEE_ADMIN_PORTAL.Features.Projects
{
    public interface IProjectService
    {
        Task<IEnumerable<Project>> GetAllProjectsAsync(int pageNo, int? pageSize, string? sortField, string? sortOrder, string? search);

        Task<Project> GetProjectByIdAsync(Guid id);

        Task<Project> AddProjectAsync(AddProjectDto addProjectDto);

        Task<Project> UpdateProjectAsync(Guid id, UpdateProjectDto updateProjectDto);

        Task<Project> DeleteProjectAsync(Guid id);

        Task<Project> MapProjectToEmployeeAsync(MapProjectEmployeeDto mapProjectEmployeeDto);
    }
}