using ASP.NET_EMPLOYEE_ADMIN_PORTAL.Exceptions;
using ASP.NET_EMPLOYEE_ADMIN_PORTAL.Features.Projects.Dtos;
using ASP.NET_EMPLOYEE_ADMIN_PORTAL.Features.Projects.Entities;

namespace ASP.NET_EMPLOYEE_ADMIN_PORTAL.Features.Projects
{
    public class ProjectService : IProjectService
    {
        private readonly IProjectRepository _projectRepository;

        public ProjectService(IProjectRepository projectRepository)
        {
            _projectRepository = projectRepository;
        }

        public async Task<IEnumerable<Project>> GetAllProjectsAsync(int pageNo, int? pageSize, string? sortField, string? sortOrder, string? search)
        {
            int size = pageSize ?? 10;
            string field = sortField ?? "Name";
            string order = string.IsNullOrEmpty(sortOrder) || sortOrder.ToLower() == "asc" || sortOrder.ToLower() == "ascending" ? "asc" : "desc";
            int toSkip = (pageNo - 1) * size;
            string sortExpression = $"{field} {order}";

            return await _projectRepository.GetAllProjects(pageNo, size, toSkip, sortExpression, search);
        }

        public async Task<Project> GetProjectByIdAsync(Guid id)
        {
            var project = await _projectRepository.GetProjectById(id);

            if (project == null)
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

            return await _projectRepository.AddProject(projectEntity);
        }

        public async Task<Project> UpdateProjectAsync(Guid id, UpdateProjectDto updateProjectDto)
        {
            var project = await _projectRepository.GetProjectById(id);

            if (project == null)
                throw new EntityNotFoundException("Project not found!");

            project.Name = updateProjectDto.Name;
            project.Description = updateProjectDto.Description;

            return await _projectRepository.UpdateProject(project);
        }

        public async Task<Project> DeleteProjectAsync(Guid id)
        {
            var project = await _projectRepository.GetProjectById(id);

            if (project == null)
                throw new EntityNotFoundException("Project not found!");

            return await _projectRepository.DeleteProject(project);
        }

        public async Task<Project> MapProjectToEmployeeAsync(MapProjectEmployeeDto mapProjectEmployeeDto)
        {
            return await _projectRepository.MapProjectToEmployee(mapProjectEmployeeDto.projectId, mapProjectEmployeeDto.employeeId);
        }
    }
}
