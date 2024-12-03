using api.Dto.Designation;
using api.Dto.Employees;
using api.Models;

namespace api.Services.IServices
{
    public interface IDesignationService:IService<DesignationDto>
    {
        Task<Response<DesignationDto>> AddAsync(DesignationCreateDto entity);
        Task<Response<DesignationDto>> UpdateAsync(int id,DesignationUpdateDto entity);
    }
}
