using api.Dto.Department;
using api.Dto.Designation;

namespace api.Services.IServices
{
    public interface IDepartmentService:IService<DepartmentDto>
    {
        Task<DepartmentDto> AddAsync(DepartmentCreateDto entity);
        Task<DepartmentDto> UpdateAsync(DepartmentUpdateDto entity);
    }
}
