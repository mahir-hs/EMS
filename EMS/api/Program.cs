using System.Net.NetworkInformation;
using api.Controllers;
using api.Data;
using api.Data.Contexts;
using api.Dto.Employees;
using api.Repository;
using api.Repository.IRepository;
using api.Services;
using api.Services.IServices;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IFactoryDbContext, FactoryDbContext>();


builder.Services.AddScoped<IEmployeeService,EmployeeService>();
builder.Services.AddScoped<IDesignationService,DesignationService>();
builder.Services.AddScoped<IDepartmentService,DepartmentService>();
builder.Services.AddScoped<IOperationLogService,OperationLogService>();
builder.Services.AddScoped<IEmployeeAttendanceService,EmployeeAttendanceService>();

builder.Services.AddScoped<IEmployeeRepository,EmployeeRepository>();
builder.Services.AddScoped<IDesignationRepository,DesignationRepository>();
builder.Services.AddScoped<IDepartmentRepository,DepartmentRepository>();
builder.Services.AddScoped<IOperationLogRepository,OperationLogRepository>();
builder.Services.AddScoped<IEmployeeAttendanceRepository,EmployeeAttendanceRepository>();


// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddControllers();
builder.Services.AddSwaggerGen(option =>
{
    option.SwaggerDoc("v1", new OpenApiInfo { Title = "Employee Management System API", Version = "v1" });
    
});
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularApp", builder =>
    {
        builder.WithOrigins("http://localhost:4200") // Update with your Angular app URL
               .AllowAnyHeader()
               .AllowAnyMethod();
    });
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}



app.UseHttpsRedirection();
app.UseRouting();

app.UseCors("AllowAngularApp"); // Apply CORS policy
app.MapControllers();

await app.RunAsync();


