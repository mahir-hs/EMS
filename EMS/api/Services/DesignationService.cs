using api.Dto.Department;
using api.Dto.Designation;
using api.Mappers;
using api.Models;
using api.Repository.IRepository;
using api.Services.IServices;

namespace api.Services
{
    public class DesignationService(IDesignationRepository context, ILogger<DesignationService> logger) : IDesignationService
    {
        private readonly IDesignationRepository _context = context;
        private readonly ILogger<DesignationService> _logger = logger;
        public async Task<Response<DesignationDto>> AddAsync(DesignationCreateDto entity)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(entity.Role))
                {
                    return new Response<DesignationDto>(
                        null,
                        false,
                        "Designation name cannot be null, empty, or whitespace.",
                        "400"
                    );
                }

                var response = await _context.AddAsync(entity.ToDesignation());

                if (!response.Success)
                {
                    return new Response<DesignationDto>(
                        null,
                        false,
                        response.Message,
                        response.Type
                    );
                }

                return new Response<DesignationDto>(
                    response.Result!.ToDesignationDto(),
                    true,
                    response.Message,
                    response.Type
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occurred while adding a Designation in service layer.");
                return new Response<DesignationDto>(
                    null,
                    false,
                    "An unexpected error occurred while adding the Designation in service layer.",
                    "500"
                );
            }
        }

        public async Task<Response<DesignationDto>> DeleteAsync(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return new Response<DesignationDto>(
                        null,
                        false,
                        "Invalid Designation ID format from service layer.",
                        "400"
                    );
                }
                var repositoryResponse = await _context.DeleteAsync(id);

                if (!repositoryResponse.Success)
                {
                    return new Response<DesignationDto>(
                        null,
                        false,
                        repositoryResponse.Message,
                        repositoryResponse.Type
                    );
                }

                return new Response<DesignationDto>(
                    repositoryResponse.Result!.ToDesignationDto(),
                    true,
                    repositoryResponse.Message,
                    repositoryResponse.Type
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occurred while deleting a Designation in service.");
                return new Response<DesignationDto>(
                    null,
                    false,
                    "An unexpected error occurred while deleting the Designation in service.",
                    "500"
                );
            }
            
            
        }

        public async Task<Response<IEnumerable<DesignationDto?>>> GetAllAsync()
        {
            try
            {
                var data = await _context.GetAllAsync();

                if (data.Success == false)
                {
                    return new Response<IEnumerable<DesignationDto?>>(
                        [],
                        false,
                        data.Message,
                        data.Type
                    );
                }

                var designationDto = data.Result.Select(x => x.ToDesignationDto());

                return new Response<IEnumerable<DesignationDto?>>(
                    designationDto,
                    true,
                    data.Message,
                    data.Type
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving Designation in service layer.");

                return new Response<IEnumerable<DesignationDto?>>(
                    [],
                    false,
                    "An error occurred while retrieving Designation in service layer.",
                    "500"
                );
            }
        }

        public async Task<Response<DesignationDto?>> GetByIdAsync(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return new Response<DesignationDto?>(
                        null,
                        false,
                        "Invalid Designation ID format from service layer.",
                        "400"
                    );
                }

                var data = await _context.GetByIdAsync(id);

                if (data.Success == false)
                {
                    return new Response<DesignationDto?>(
                        null,
                        false,
                        data.Message,
                        data.Type
                    );
                }

                return new Response<DesignationDto?>(
                    data.Result.ToDesignationDto(),
                    true,
                    data.Message,
                    data.Type
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving the Designation by ID in service layer.");

                return new Response<DesignationDto?>(
                    null,
                    false,
                    "An error occurred while retrieving the Designation in service layer.",
                    "500"
                );
            }
        }

        public async Task<Response<DesignationDto>> UpdateAsync(int id, DesignationUpdateDto entity)
        {
            try
            {
                if (id <= 0)
                {
                    return new Response<DesignationDto>(
                        null,
                        false,
                        "Invalid department ID format from service layer.",
                        "400"
                    );
                }
                var fetch = await _context.GetByIdAsync(id);

                if (fetch.Success == false)
                {
                    return new Response<DesignationDto>(
                        null,
                        false,
                        fetch.Message,
                        fetch.Type
                    );
                }

                var update = await _context.UpdateAsync(id, entity.ToDesignation(fetch.Result!));

                if (update.Success == false)
                {
                    return new Response<DesignationDto>(
                        null,
                        false,
                        update.Message,
                        update.Type
                    );
                }

                return new Response<DesignationDto>(
                    update.Result.ToDesignationDto(),
                    true,
                    update.Message,
                    update.Type
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating the Designation service layer.");

                return new Response<DesignationDto>(
                    null,
                    false,
                    "An unexpected error occurred while updating the Designation in service layer.",
                    "500"
                );
            }
           
            
        }
    }
}
