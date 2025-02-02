using ASP.NET_EMPLOYEE_ADMIN_PORTAL.Data;
using ASP.NET_EMPLOYEE_ADMIN_PORTAL.Exceptions;
using ASP.NET_EMPLOYEE_ADMIN_PORTAL.Features.Employees;
using ASP.NET_EMPLOYEE_ADMIN_PORTAL.Features.Employees.Entities;
using ASP.NET_EMPLOYEE_ADMIN_PORTAL.Features.Offices;
using ASP.NET_EMPLOYEE_ADMIN_PORTAL.Features.Offices.Entities;
using ASP.NET_EMPLOYEE_ADMIN_PORTAL.Features.Projects;
using ASP.NET_EMPLOYEE_ADMIN_PORTAL.Features.Projects.Entities;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Exceptions
builder.Services.AddExceptionHandler<EntityNotFoundExceptionHandler>();
builder.Services.AddExceptionHandler<ArgumentExceptionHandler>();
builder.Services.AddExceptionHandler<InvalidOperationExceptionHandler>();
builder.Services.AddExceptionHandler<AppExceptionHandler>();
builder.Services.AddProblemDetails();

// Db
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Services
builder.Services.AddScoped<IEmployeeService, EmployeeService>();
builder.Services.AddScoped<IOfficeService, OfficeService>();
builder.Services.AddScoped<IProjectService, ProjectService>();

// Repositories
builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
builder.Services.AddScoped<IOfficeRepository, OfficeRepository>();
builder.Services.AddScoped<IProjectRepository, ProjectRepository>();

// Validators
builder.Services.AddScoped<IValidator<Employee>, EmployeeValidator>();
builder.Services.AddScoped<IValidator<Office>, OfficeValidator>();
builder.Services.AddScoped<IValidator<Project>, ProjectValidator>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseExceptionHandler();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
