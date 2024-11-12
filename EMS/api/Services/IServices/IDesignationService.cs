using api.Dtos.Designation;
using api.Dtos.Employees;

namespace api.Services.IServices
{
    public interface IDesignationService:IService<DesignationDto>
    {
        Task<DesignationDto> AddAsync(DesignationCreateDto entity);
        Task<DesignationDto> UpdateAsync(DesignationUpdateDto entity);
    }
}
