using api.Dtos.Department;
using api.Mappers;
using api.Repository.IRepository;
using api.Services.IServices;

namespace api.Services
{
    public class DepartmentService(IDepartmentRepository context) : IDepartmentService
    {
        private readonly IDepartmentRepository _context = context;
        public async Task<DepartmentDto> AddAsync(DepartmentCreateDto entity)
        {
            var fetch = await _context.AddAsync(entity.ToDepartment());
            return fetch!.ToDepartmentDto();
        }

        public async Task<DepartmentDto> DeleteAsync(int id)
        {
            var fetch = await _context.DeleteAsync(id);
            return fetch!.ToDepartmentDto();
        }

        public async Task<IEnumerable<DepartmentDto?>> GetAllAsync()
        {
            var data = await _context.GetAllAsync();
            if (data == null)
            {
                return [];
            }
            return data.Select(x => x.ToDepartmentDto());
        }

        public async Task<DepartmentDto?> GetByIdAsync(int id)
        {
            var data = await _context.GetByIdAsync(id);
            return data?.ToDepartmentDto();
        }

        public async Task<DepartmentDto> UpdateAsync(DepartmentUpdateDto entity)
        {
            var fetch = await _context.GetByIdAsync(entity.Id);
            var data = entity.ToDepartment(fetch!);
            fetch = await _context.UpdateAsync(data);
            if (fetch == null)
            {
                throw new ArgumentNullException(nameof(fetch),
                                                "Designation not found for the given Id.");
            }

            return fetch!.ToDepartmentDto();
        }
    }
}
