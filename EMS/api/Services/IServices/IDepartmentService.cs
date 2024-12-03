using api.Dto.Department;
using api.Dto.Designation;
using api.Models;

namespace api.Services.IServices
{
    public interface IDepartmentService:IService<DepartmentDto>
    {
        Task<Response<DepartmentDto>> AddAsync(DepartmentCreateDto entity);
        Task<Response<DepartmentDto>> UpdateAsync(int id, DepartmentUpdateDto entity);
    }
}
