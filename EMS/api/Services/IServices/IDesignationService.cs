using api.Dto.Designation;
using api.Dto.Employees;

namespace api.Services.IServices
{
    public interface IDesignationService:IService<DesignationDto>
    {
        Task<DesignationDto> AddAsync(DesignationCreateDto entity);
        Task<DesignationDto> UpdateAsync(int id,DesignationUpdateDto entity);
    }
}
