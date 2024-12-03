using api.Dto.Department;
using api.Dto.Designation;
using api.Mappers;
using api.Models;
using api.Repository.IRepository;
using api.Services.IServices;
using Azure;

namespace api.Services
{
    public class DesignationService(IDesignationRepository context, ILogger<DesignationService> logger) : IDesignationService
    {
        private readonly IDesignationRepository _context = context;
        private readonly ILogger<DesignationService> _logger = logger;
        public async Task<ApiResponse> AddAsync(DesignationCreateDto entity)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(entity.Role))
                {
                    return new ApiResponse(
                        null,
                        false,
                        "Designation name cannot be null, empty, or whitespace.",
                        "400"
                    );
                }

                var response = await _context.AddAsync(entity.ToDesignation());

                if (!response.Success)
                {
                    return new ApiResponse(
                        null,
                        false,
                        response.Message,
                        response.Type
                    );
                }
                var designation = response.Result as Designation;
                return new ApiResponse(
                    designation.ToDesignationDto(),
                    true,
                    response.Message,
                    response.Type
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occurred while adding a Designation in service layer.");
                return new ApiResponse(
                    null,
                    false,
                    "An unexpected error occurred while adding the Designation in service layer.",
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
                        "Invalid Designation ID format from service layer.",
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
                _logger.LogError(ex, "Unexpected error occurred while deleting a Designation in service.");
                return new ApiResponse(
                    null,
                    false,
                    "An unexpected error occurred while deleting the Designation in service.",
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

                var designations = data.Result as IEnumerable<Designation>;
                var designationDto = designations?.Select(x => x.ToDesignationDto());

                return new ApiResponse(
                    designationDto,
                    true,
                    data.Message,
                    data.Type
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving Designation in service layer.");

                return new ApiResponse(
                    null,
                    false,
                    "An error occurred while retrieving Designation in service layer.",
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
                        "Invalid Designation ID format from service layer.",
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

                var designation = data.Result as Designation;
                var designationDto = designation.ToDesignationDto();

                return new ApiResponse(
                    designationDto,
                    true,
                    data.Message,
                    data.Type
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving the Designation by ID in service layer.");

                return new ApiResponse(
                    null,
                    false,
                    "An error occurred while retrieving the Designation in service layer.",
                    "500"
                );
            }
        }

        public async Task<ApiResponse> UpdateAsync(int id, DesignationUpdateDto entity)
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

                var data = fetch.Result as Designation;

                var update = await _context.UpdateAsync(id, entity.ToDesignation(data!));

                if (!update.Success)
                {
                    return new ApiResponse(
                        null,
                        false,
                        update.Message,
                        update.Type
                    );
                }
                var designation = update.Result as Designation;
                return new ApiResponse(
                    designation.ToDesignationDto(),
                    true,
                    update.Message,
                    update.Type
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating the Designation service layer.");

                return new ApiResponse(
                    null,
                    false,
                    "An unexpected error occurred while updating the Designation in service layer.",
                    "500"
                );
            }
           
            
        }
    }
}
