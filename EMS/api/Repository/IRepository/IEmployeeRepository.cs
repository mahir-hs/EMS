using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Models;

namespace api.Repository.IRepository
{
    public interface IEmployeeRepository:IRepository<Employee>
    {
        Task<Employee?> UpdateAsync(Employee entity);
    }
}