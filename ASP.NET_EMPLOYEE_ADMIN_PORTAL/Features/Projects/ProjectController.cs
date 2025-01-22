using ASP.NET_EMPLOYEE_ADMIN_PORTAL.Features.Projects;
using ASP.NET_EMPLOYEE_ADMIN_PORTAL.Features.Projects.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace ASP.NET_EMPLOYEE_ADMIN_PORTAL.Features.Projects
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProjectController : Controller
    {
        private readonly IProjectService _projectsService;

        public ProjectController(IProjectService projectsService)
        {
            _projectsService = projectsService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllProjects(
            [FromQuery(Name = "pageNo")] int pageNo,
            [FromQuery(Name = "pageSize")] int? pageSize,
            [FromQuery(Name = "sortField")] string? sortField,
            [FromQuery(Name = "sortOrder")] string? sortOrder,
            [FromQuery(Name = "search")] string? search)
        {
            var projects = await _projectsService.GetAllProjectsAsync(pageNo, pageSize, sortField, sortOrder, search);

            return Ok(projects);
        }

        [HttpGet]
        [Route("{id:guid}")]
        public async Task<IActionResult> GetProjectById(Guid id)
        {
            var project = await _projectsService.GetProjectByIdAsync(id);

            return Ok(project);
        }

        [HttpPost]
        public async Task<IActionResult> AddProject(AddProjectDto addProjectDto)
        {
            var project = await _projectsService.AddProjectAsync(addProjectDto);

            return Ok(project);
        }

        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> UpdateProject(Guid id, UpdateProjectDto updateProjectDto)
        {
            var project = await _projectsService.UpdateProjectAsync(id, updateProjectDto);

            return Ok(project);
        }

        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> DeleteProject(Guid id)
        {
            var project = await _projectsService.DeleteProjectAsync(id);

            return Ok(project);
        }

        [HttpPost]
        [Route("map")]
        public async Task<IActionResult> MapProjectToEmployee(MapProjectEmployeeDto mapProjectEmployeeDto)
        {
            var project = await _projectsService.MapProjectToEmployeeAsync(mapProjectEmployeeDto);

            return Ok(new
            {
                project.Name,
                project.Description,
                Employees = project.Employees.Select(e => e.Name)
            });
        }
    }
}
