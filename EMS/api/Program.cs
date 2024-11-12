using System.Net.NetworkInformation;
using api.Data.Contexts;
using api.Repository;
using api.Repository.IRepository;
using api.Services;
using api.Services.IServices;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IDapperContext,SqlServerDapperContext>();

builder.Services.AddScoped<IEmployeeService,EmployeeService>();
builder.Services.AddScoped<IDesignationService,DesignationService>();
builder.Services.AddScoped<IDepartmentService,DepartmentService>();

builder.Services.AddScoped<IEmployeeRepository,EmployeeRepository>();
builder.Services.AddScoped<IDesignationRepository,DesignationRepository>();
builder.Services.AddScoped<IDepartmentRepository,DepartmentRepository>();

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddControllers();
builder.Services.AddSwaggerGen(option =>
{
    option.SwaggerDoc("v1", new OpenApiInfo { Title = "Employee Management System API", Version = "v1" });
    
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

app.MapControllers();

app.Run();


