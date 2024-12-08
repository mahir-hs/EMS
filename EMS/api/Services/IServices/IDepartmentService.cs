using api.Dto.Department;
using api.Models;

namespace api.Services.IServices
{
    public interface IDepartmentService:IService<DepartmentDto>
    {
        Task<ApiResponse> AddAsync(DepartmentCreateDto entity);
        Task<ApiResponse> UpdateAsync(int id, DepartmentUpdateDto entity);
    }
}
