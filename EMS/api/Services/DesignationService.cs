using api.Dto.Designation;
using api.Mappers;
using api.Repository.IRepository;
using api.Services.IServices;

namespace api.Services
{
    public class DesignationService(IDesignationRepository context) : IDesignationService
    {
        private readonly IDesignationRepository _context = context;
        public async Task<DesignationDto> AddAsync(DesignationCreateDto entity)
        {
            var fetch = await _context.AddAsync(entity.ToDesignation());
            return fetch!.ToDesignationDto();
            
        }

        public async Task<DesignationDto> DeleteAsync(int id)
        {
            var fetch = await _context.DeleteAsync(id);
            return fetch!.ToDesignationDto();
            
        }

        public async Task<IEnumerable<DesignationDto?>> GetAllAsync()
        {
            var data = await _context.GetAllAsync();
            if (data == null)
            {
                return [];
            }
            return data.Select(x => x.ToDesignationDto());
            
        }

        public async Task<DesignationDto?> GetByIdAsync(int id)
        {
            var data = await _context.GetByIdAsync(id);
            return data?.ToDesignationDto();
           
        }

        public async Task<DesignationDto> UpdateAsync(int id, DesignationUpdateDto entity)
        {
            var fetch = await _context.GetByIdAsync(id);
            var data = entity.ToDesignation(fetch!);
            fetch = await _context.UpdateAsync(id,data);
            if (fetch == null)
            {
                throw new ArgumentNullException(nameof(fetch),
                                                "Designation not found for the given Id.");
            }

            return fetch!.ToDesignationDto();
            
        }
    }
}
