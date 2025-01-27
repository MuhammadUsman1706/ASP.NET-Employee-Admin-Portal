using ASP.NET_EMPLOYEE_ADMIN_PORTAL.Exceptions;
using ASP.NET_EMPLOYEE_ADMIN_PORTAL.Features.Projects.Dtos;
using ASP.NET_EMPLOYEE_ADMIN_PORTAL.Features.Projects.Entities;
using FluentValidation;

namespace ASP.NET_EMPLOYEE_ADMIN_PORTAL.Features.Projects
{
    public class ProjectService : IProjectService
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IValidator<Project> _projectValidator;

        public ProjectService(IProjectRepository projectRepository, IValidator<Project> projectValidator)
        {
            _projectRepository = projectRepository;
            _projectValidator = projectValidator;
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

            var validationResult = await _projectValidator.ValidateAsync(projectEntity);

            if (!validationResult.IsValid)
            {
                var errors = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
                throw new ValidationException($"Project validation failed: {errors}");
            }

            return await _projectRepository.AddProject(projectEntity);
        }

        public async Task<Project> UpdateProjectAsync(Guid id, UpdateProjectDto updateProjectDto)
        {
            var project = await _projectRepository.GetProjectById(id);

            if (project == null)
                throw new EntityNotFoundException("Project not found!");

            project.Name = updateProjectDto.Name;
            project.Description = updateProjectDto.Description;

            var validationResult = await _projectValidator.ValidateAsync(project);

            if (!validationResult.IsValid)
            {
                var errors = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
                throw new ValidationException($"Project validation failed: {errors}");
            }

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
