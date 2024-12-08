using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dto.Employees;
using api.Mappers;
using api.Models;
using api.Repository;
using api.Repository.IRepository;
using api.Services.IServices;

namespace api.Services
{
    public class EmployeeService(IEmployeeRepository context,ILogger<IEmployeeService> logger) : IEmployeeService
    {
        private readonly IEmployeeRepository _context = context;
        private readonly ILogger<IEmployeeService> _logger = logger;
        
        public async Task<ApiResponse> AddAsync(EmployeeCreateDto entity)
        {
            var validationErrors = EmployeeCreateDtoValidator.Validate(entity);
            if (validationErrors.Count != 0)
            {
                return new ApiResponse(
                    null,
                    false,
                    $"Validation errors: {string.Join("; ", validationErrors)}",
                    "400"
                );
            }
            
            try
            {
                var response = await _context.AddAsync(entity.ToEmployee());
                if (!response.Success)
                {
                    return new ApiResponse(
                        null,
                        false,
                        response.Message,
                        response.Type
                    );
                }
                var data = response.Result as Employee;
               
                return new ApiResponse(
                    data.ToEmployeeDto(),
                    true,
                    response.Message,
                    response.Type
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while adding an employee");
                return new ApiResponse(
                    null,
                    false,
                    ex.Message,
                    "500"
                );
            }

        }

        public async Task<ApiResponse> DeleteAsync(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return new ApiResponse(
                        null,
                        false,
                        "Invalid employee ID format from service layer.",
                        "400"
                    );
                }
                var repositoryResponse = await _context.DeleteAsync(id);
                if (!repositoryResponse.Success)
                {
                    return new ApiResponse(
                        null,
                        false,
                        repositoryResponse.Message,
                        repositoryResponse.Type
                    );
                }

                return new ApiResponse(
                    null,
                    true,
                    repositoryResponse.Message,
                    repositoryResponse.Type
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting an employee");
                return new ApiResponse(
                    null,
                    false,
                    ex.Message,
                    "500"
                );
            }

        }

        public async Task<ApiResponse> GetAllAsync()
        {
            try
            {
                var response = await _context.GetAllAsync();
                if (!response.Success)
                {
                    return new ApiResponse(
                        null,
                        false,
                        response.Message,
                        response.Type
                    );
                }
                var employees = response.Result as IEnumerable<Employee>;
                var employeesDto = employees?.Select(x => x.ToEmployeeDto());
                return new ApiResponse(
                    employeesDto,
                    true,
                    response.Message,
                    response.Type
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving employees in service layer.");
                return new ApiResponse(
                    null,
                    false,
                    ex.Message,
                    "500"
                );
            }
        }

        public async Task<ApiResponse> GetByIdAsync(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return new ApiResponse(
                        null,
                        false,
                        "Invalid employee ID format from service layer.",
                        "400"
                    );
                }

                var response = await _context.GetByIdAsync(id);
                if (!response.Success)
                {
                    return new ApiResponse(
                        null,
                        false,
                        response.Message,
                        response.Type
                    );
                }
                var employee = response.Result as Employee;
                return new ApiResponse(
                    employee.ToEmployeeDto(),
                    true,
                    response.Message,
                    response.Type
                );

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving an employee in service layer.");
                return new ApiResponse(
                    null,
                    false,
                    ex.Message,
                    "500"
                );
            }
            
        }

        public async Task<ApiResponse> UpdateAsync(int id, EmployeeUpdateDto entity)
        {
            try
            {
                if (id <= 0)
                {
                    return new ApiResponse(
                        null,
                        false,
                        "Invalid employee ID format from service layer.",
                        "400"
                    );
                }

                var validationErrors = EmployeeUpdateDtoValidator.Validate(entity);
                if (validationErrors.Count != 0)
                {
                    return new ApiResponse(
                        null,
                        false,
                        $"Validation errors: {string.Join("; ", validationErrors)}",
                        "400"
                    );
                }

                var fetch = await _context.GetByIdAsync(id);
                var data = fetch.Result as Employee;
                var repositoryResponse = await _context.UpdateAsync(id, entity.ToEmployee(data!));
                if (!repositoryResponse.Success)
                {
                    return new ApiResponse(
                        null,
                        false,
                        repositoryResponse.Message,
                        repositoryResponse.Type
                    );
                }
                var dataEmployee = repositoryResponse.Result as Employee;
                return new ApiResponse(
                    dataEmployee.ToEmployeeDto(),
                    true,
                    repositoryResponse.Message,
                    repositoryResponse.Type
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating an employee in service layer.");
                return new ApiResponse(
                    null,
                    false,
                    ex.Message,
                    "500"
                );
            }

        }
    }
}