using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dto.Employees;
using api.Models;

namespace api.Services.IServices
{
    public interface IEmployeeService:IService<EmployeeDto>
    {
        Task<EmployeeDto> AddAsync(EmployeeCreateDto entity);
        Task<EmployeeDto> UpdateAsync(EmployeeUpdateDto entity);
    }
}