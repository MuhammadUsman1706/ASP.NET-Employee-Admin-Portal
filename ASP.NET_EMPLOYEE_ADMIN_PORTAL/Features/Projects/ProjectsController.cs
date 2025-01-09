using ASP.NET_EMPLOYEE_ADMIN_PORTAL.Data;
using ASP.NET_EMPLOYEE_ADMIN_PORTAL.Features.Employees.Entities;
using ASP.NET_EMPLOYEE_ADMIN_PORTAL.Features.Projects.Dtos;
using ASP.NET_EMPLOYEE_ADMIN_PORTAL.Features.Projects.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;

namespace ASP.NET_EMPLOYEE_ADMIN_PORTAL.Features.Projects
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProjectsController : Controller
    {
        private readonly ApplicationDbContext dbContext;

        public ProjectsController(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet]
        public IActionResult GetAllProjects(
            [FromQuery(Name = "pageNo")] int pageNo,
            [FromQuery(Name = "pageSize")] int? pageSize,
            [FromQuery(Name = "sortField")] string? sortField,
            [FromQuery(Name = "sortOrder")] string? sortOrder,
            [FromQuery(Name = "search")] string? search)
        {
            int size = pageSize ?? 10;
            string field = sortField ?? "Name";
            string order = string.IsNullOrEmpty(sortOrder) || sortOrder.ToLower() == "ascending" || sortOrder.ToLower() == "asc" ? "asc" : "desc";

            if (pageNo <= 0)
                return BadRequest("Page number must be greater than 0");


            int toSkip = (pageNo - 1) * size;

            // Sort expression: "Name asc" or "Name desc"
            string sortExpression = $"{field} {order}";

            try
            {
                var allProjects = dbContext.Projects
                    .Select(project => new Project()
                    {
                        Id = project.Id,
                        Name = project.Name,
                        Description = project.Description,
                        Employees = project.Employees.Select(e => new Employee()
                        {
                            Id = e.Id,
                            Name = e.Name,
                            Email = e.Email,
                            Phone = e.Phone,
                            Salary = e.Salary,
                            OfficeId = e.OfficeId,
                        }).ToList()
                    })
                    .Where(p => search == null || p.Name.Contains(search) || p.Description.Contains(search))
                    .OrderBy(sortExpression)
                    .Skip(toSkip)
                    .Take(size)
                    .ToList<Project>();

                return Ok(allProjects);
            }
            catch (Exception ex)
            {
                // Handle invalid field names or other errors
                return BadRequest(new { message = $"Invalid sort field or order: {ex.Message}" });
            }
        }

        [HttpGet]
        [Route("{id:guid}")]
        public IActionResult GetProjectById(Guid id)
        {
            var project = dbContext.Projects.Select(project => new Project()
            {
                Id = project.Id,
                Name = project.Name,
                Description = project.Description,
                Employees = project.Employees.Select(e => new Employee()
                {
                    Id = e.Id,
                    Name = e.Name,
                    Email = e.Email,
                    Phone = e.Phone,
                    Salary = e.Salary,
                    OfficeId = e.OfficeId,
                }).ToList()
            }).FirstOrDefault(p => p.Id == id);

            if (project is null)
                return NotFound("Project not found!");

            return Ok(project);
        }

        [HttpPost]
        public IActionResult AddProject(AddProjectDto addProjectDto)
        {
            var projectEntity = new Project()
            {
                Name = addProjectDto.Name,
                Description = addProjectDto.Description
            };

            dbContext.Projects.Add(projectEntity);
            dbContext.SaveChanges();

            return Ok(addProjectDto);
        }

        [HttpPut]
        [Route("{id:guid}")]
        public IActionResult UpdateProject(Guid id, UpdateProjectDto updateProjectDto)
        {
            var project = dbContext.Projects.Find(id);

            if (project is null)
                return NotFound("Project not found!");

            project.Name = updateProjectDto.Name;
            project.Description = updateProjectDto.Description;

            dbContext.SaveChanges();

            return Ok(project);
        }

        [HttpDelete]
        [Route("{id:guid}")]
        public IActionResult DeleteProject(Guid id)
        {
            var project = dbContext.Projects.Find(id);

            if (project is null)
                return NotFound("Project not found!");

            dbContext.Projects.Remove(project);
            dbContext.SaveChanges();

            return Ok(project);
        }

        [HttpPost]
        [Route("map")]
        public IActionResult MapProjectToEmployee(MapProjectEmployeeDto mapProjectEmployeeDto)
        {
            var project = dbContext.Projects.Include(p => p.Employees).FirstOrDefault(p => p.Id == mapProjectEmployeeDto.projectId);

            if (project is null)
                return NotFound("Project not found!");

            var employee = dbContext.Employees.Find(mapProjectEmployeeDto.employeeId);

            if (employee is null)
                return NotFound("Employee not found!");

            project.Employees.Add(employee);

            dbContext.SaveChanges();

            var newProject = new
            {
                project.Name,
                project.Description,
                Employees = project.Employees.Select(x => x.Name),
            };

            return Ok(newProject);
        }
    }
}
