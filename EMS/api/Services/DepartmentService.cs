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
        public async Task<Response<DepartmentDto>> AddAsync(DepartmentCreateDto entity)
        {

            try
            {
                if (string.IsNullOrWhiteSpace(entity.Dept))
                {
                    return new Response<DepartmentDto>(
                        null,
                        false,
                        "Department name cannot be null, empty, or whitespace.",
                        "500"
                    );
                }

                var response = await _context.AddAsync(entity.ToDepartment());

                if (!response.Success)
                {
                    return new Response<DepartmentDto>(
                        null,
                        false,
                        response.Message,
                        response.Type
                    );
                }

                return new Response<DepartmentDto>(
                    response.Result!.ToDepartmentDto(),
                    true,
                    response.Message,
                    response.Type
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occurred while adding a department in service layer.");
                return new Response<DepartmentDto>(
                    null,
                    false,
                    "An unexpected error occurred while adding the department in service layer.",
                    "500"
                );
            }


        }

        public async Task<Response<DepartmentDto>> DeleteAsync(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return new Response<DepartmentDto>(
                        null,
                        false,
                        "Invalid department ID format from service layer.",
                        "400"
                    );
                }
                var repositoryResponse = await _context.DeleteAsync(id);

                if (!repositoryResponse.Success)
                {
                    return new Response<DepartmentDto>(
                        null,
                        false,
                        repositoryResponse.Message,
                        repositoryResponse.Type
                    );
                }

                return new Response<DepartmentDto>(
                    repositoryResponse.Result!.ToDepartmentDto(),
                    true,
                    repositoryResponse.Message,
                    repositoryResponse.Type
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occurred while deleting a department in service.");
                return new Response<DepartmentDto>(
                    null,
                    false,
                    "An unexpected error occurred while deleting the department in service.",
                    "500"
                );
            }
        }

        public async Task<Response<IEnumerable<DepartmentDto?>>> GetAllAsync()
        {
            try
            {
                var data = await _context.GetAllAsync();

                if (!data.Success)
                {
                    return new Response<IEnumerable<DepartmentDto?>>(
                        [], 
                        false,                              
                        data.Message,             
                        data.Type                                
                    );
                }
                
                var departmentDtos = data.Result.Select(x => x.ToDepartmentDto());

                return new Response<IEnumerable<DepartmentDto?>>(
                    departmentDtos,                         
                    true,
                    data.Message,
                    data.Type
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving departments in service layer.");

                return new Response<IEnumerable<DepartmentDto?>>(
                    [],
                    false,
                    "An error occurred while retrieving departments in service layer.",
                    "500"
                );
            }
        }

        public async Task<Response<DepartmentDto?>> GetByIdAsync(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return new Response<DepartmentDto?>(
                        null,
                        false,
                        "Invalid department ID format from service layer.",
                        "400"
                    );
                }

                var data = await _context.GetByIdAsync(id);

                if (!data.Success)
                {
                    return new Response<DepartmentDto?>(
                        null,                              
                        false,                              
                        data.Message,            
                        data.Type
                    );
                }

                return new Response<DepartmentDto?>(
                    data.Result.ToDepartmentDto(),
                    true,                           
                    data.Message, 
                    data.Type                                
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving the department by ID in service layer.");

                return new Response<DepartmentDto?>(
                    null,                              
                    false,                              
                    "An error occurred while retrieving the department in service layer.",
                    "500"                              
                );
            }
        }

        public async Task<Response<DepartmentDto>> UpdateAsync(int id, DepartmentUpdateDto entity)
        {

            try
            {
                if (id <= 0)
                {
                    return new Response<DepartmentDto>(
                        null,
                        false,
                        "Invalid department ID format from service layer.",
                        "400"
                    );
                }
                var fetch = await _context.GetByIdAsync(id);

                if (!fetch.Success)
                {
                    return new Response<DepartmentDto>(
                        null,
                        false,
                        fetch.Message,
                        fetch.Type
                    );
                }

                var updatedDepartment = await _context.UpdateAsync(id, entity.ToDepartment(fetch.Result!));

                if (!updatedDepartment.Success)
                {
                    return new Response<DepartmentDto>(
                        null,
                        false,
                        updatedDepartment.Message,
                        updatedDepartment.Type
                    );
                }

                return new Response<DepartmentDto>(
                    updatedDepartment.Result.ToDepartmentDto(),
                    true,
                    updatedDepartment.Message,
                    updatedDepartment.Type
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating the department service layer.");

                return new Response<DepartmentDto>(
                    null,
                    false,
                    "An unexpected error occurred while updating the department in service layer.",
                    "500"
                );
            }
        }

        
    }
}
