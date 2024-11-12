using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos.Employees;
using api.Models;

namespace api.Mappers
{
    public static class EmployeeMapper
    {
        public static EmployeeDto ToEmployeeDto(this Employee? employee)
        {
            if(employee==null)
            {
               throw new ArgumentNullException(nameof(employee), "Employee cannot be null.");
            }

            return new EmployeeDto{
              Id = employee.Id,
              FirstName = employee.FirstName,
              LastName = employee.LastName,
              Email = employee.Email,
              Phone = employee.Phone,
              Address = employee.Address,
              DateOfBirth = employee.DateOfBirth,
              DepartmentId = employee.DepartmentId,
              DesignationId = employee.DesignationId
            };
        }

        public static Employee ToEmployee(this EmployeeCreateDto dto)
        {
            return new Employee{
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                Phone = dto.Phone,
                Address = dto.Address,
                DateOfBirth = dto.DateOfBirth,
                DepartmentId = dto?.DepartmentId,
                DesignationId = dto?.DesignationId
            };
        }

        public static Employee ToEmployee(this EmployeeUpdateDto dto,Employee employee)
        {
            if(dto.FirstName!=null) employee.FirstName = dto.FirstName;
            if(dto.LastName!=null) employee.LastName = dto.LastName;
            if(dto.Email!=null) employee.Email = dto.Email;
            if(dto.Phone!=null) employee.Phone = dto.Phone;
            if(dto.Address!=null) employee.Address = dto.Address;
            if(dto.DateOfBirth.HasValue) employee.DateOfBirth = dto.DateOfBirth.Value;
            if(dto.DepartmentId.HasValue) employee.DepartmentId = dto.DepartmentId.Value;
            if(dto.DesignationId.HasValue) employee.DesignationId = dto.DesignationId.Value;

            return employee;

        }
    }
}