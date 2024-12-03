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
        Task<Response<EmployeeDto>> AddAsync(EmployeeCreateDto entity);
        Task<Response<EmployeeDto>> UpdateAsync(int id,EmployeeUpdateDto entity);
    }
}