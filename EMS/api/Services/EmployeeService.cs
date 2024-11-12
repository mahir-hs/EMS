using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos.Employees;
using api.Mappers;
using api.Models;
using api.Repository;
using api.Repository.IRepository;
using api.Services.IServices;

namespace api.Services
{
    public class EmployeeService(IEmployeeRepository context) : IEmployeeService
    {
        private readonly IEmployeeRepository _context = context;
        
        public async Task<EmployeeDto> AddAsync(EmployeeCreateDto entity)
        {
            
            var data = entity.ToEmployee();
            var fetch = await _context.AddAsync(data);
            var toDto = fetch.ToEmployeeDto();
            return toDto;

        }

        public async Task<EmployeeDto> DeleteAsync(int id)
        {
            
            var fetch = await _context.DeleteAsync(id);
            var toDto = fetch!.ToEmployeeDto();
            return toDto;
        }

        public async Task<IEnumerable<EmployeeDto?>> GetAllAsync()
        {
            var data = await _context.GetAllAsync();
            if(data==null)
            {
                return []; 
            }
            var toDto = data.Select(x =>x.ToEmployeeDto());
            return toDto;
        }

        public async Task<EmployeeDto?> GetByIdAsync(int id)
        {
            var data = await _context.GetByIdAsync(id);
            var toDto = data?.ToEmployeeDto();
            return toDto;
        }

        public async Task<EmployeeDto> UpdateAsync(EmployeeUpdateDto entity)
        {
            
            var fetch = await _context.GetByIdAsync(entity.Id);
            var data = entity.ToEmployee(fetch!);
            fetch = await _context.UpdateAsync(data);
            if (fetch == null)
            {
                throw new ArgumentNullException(nameof(fetch),
                                                "Employee not found for the given Id.");
            }
            var toDto = fetch!.ToEmployeeDto();
            return toDto;
        }
    }
}