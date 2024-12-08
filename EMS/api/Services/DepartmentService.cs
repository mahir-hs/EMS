using api.Dto.Department;
using api.Mappers;
using api.Models;
using api.Repository.IRepository;
using api.Services.IServices;

namespace api.Services
{
    public class DepartmentService(IDepartmentRepository context, ILogger<DepartmentService> logger) : IDepartmentService
    {
        private readonly IDepartmentRepository _context = context;
        private readonly ILogger<DepartmentService> _logger = logger;
        public async Task<ApiResponse> AddAsync(DepartmentCreateDto entity)
        {

            try
            {
                if (string.IsNullOrWhiteSpace(entity.Dept))
                {
                    return new ApiResponse(
                        null,
                        false,
                        "Department name cannot be null, empty, or whitespace.",
                        "500"
                    );
                }


                var response = await _context.AddAsync(entity.ToDepartment());

                if (!response.Success)
                {
                    return new ApiResponse(
                        null,
                        false,
                        response.Message,
                        response.Type
                    );
                }

                var data = response.Result as Department;

                return new ApiResponse(
                    data.ToDepartmentDto(),
                    true,
                    response.Message,
                    response.Type
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occurred while adding a department in service layer.");
                return new ApiResponse(
                    null,
                    false,
                    "An unexpected error occurred while adding the department in service layer.",
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
                        "Invalid department ID format from service layer.",
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
                    true,
                    true,
                    repositoryResponse.Message,
                    repositoryResponse.Type
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occurred while deleting a department in service.");
                return new ApiResponse(
                    null,
                    false,
                    "An unexpected error occurred while deleting the department in service.",
                    "500"
                );
            }
        }

        public async Task<ApiResponse> GetAllAsync()
        {
            try
            {
                var data = await _context.GetAllAsync();

                if (!data.Success)
                {
                    return new ApiResponse(
                        null, 
                        false,                              
                        data.Message,             
                        data.Type                                
                    );
                }
                
                var departments = data.Result as IEnumerable<Department>;
                var departmentDtos = departments?.Select(x => x.ToDepartmentDto());

                return new ApiResponse(
                    departmentDtos,                         
                    true,
                    data.Message,
                    data.Type
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving departments in service layer.");

                return new ApiResponse(
                    null,
                    false,
                    "An error occurred while retrieving departments in service layer.",
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
                        "Invalid department ID format from service layer.",
                        "400"
                    );
                }

                var data = await _context.GetByIdAsync(id);

                if (!data.Success)
                {
                    return new ApiResponse(
                        null,                              
                        false,                              
                        data.Message,            
                        data.Type
                    );
                }
                var department = data.Result as Department;
                var departmentDto = department.ToDepartmentDto();
                return new ApiResponse(
                    departmentDto,
                    true,                           
                    data.Message, 
                    data.Type                                
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving the department by ID in service layer.");

                return new ApiResponse(
                    null,                              
                    false,                              
                    "An error occurred while retrieving the department in service layer.",
                    "500"                              
                );
            }
        }

        public async Task<ApiResponse> UpdateAsync(int id, DepartmentUpdateDto entity)
        {

            try
            {
                if (id <= 0)
                {
                    return new ApiResponse(
                        null,
                        false,
                        "Invalid department ID format from service layer.",
                        "400"
                    );
                }
                var fetch = await _context.GetByIdAsync(id);

                if (!fetch.Success)
                {
                    return new ApiResponse(
                        null,
                        false,
                        fetch.Message,
                        fetch.Type
                    );
                }

                var updatedDepartment = await _context.UpdateAsync(id, entity.ToDepartment((Department)fetch.Result!));

                if (!updatedDepartment.Success)
                {
                    return new ApiResponse(
                        null,
                        false,
                        updatedDepartment.Message,
                        updatedDepartment.Type
                    );
                }

                var data = updatedDepartment.Result as Department;

                return new ApiResponse(
                    data.ToDepartmentDto(),
                    true,
                    updatedDepartment.Message,
                    updatedDepartment.Type
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating the department service layer.");

                return new ApiResponse(
                    null,
                    false,
                    "An unexpected error occurred while updating the department in service layer.",
                    "500"
                );
            }
        }

        
    }
}
