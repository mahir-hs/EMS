using api.Dto.Designation;
using api.Dto.Employees;
using api.Models;
using api.Models;

namespace api.Services.IServices
{
    public interface IDesignationService:IService<DesignationDto>
    {
        Task<ApiResponse> AddAsync(DesignationCreateDto entity);
        Task<ApiResponse> UpdateAsync(int id,DesignationUpdateDto entity);
    }
}
