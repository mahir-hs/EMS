using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using api.Data.Contexts;
using api.Models;
using api.Repository.IRepository;
using Dapper;

namespace api.Repository
{
    public class EmployeeRepository:IEmployeeRepository
    {
        private readonly IDapperContext _context;
        public EmployeeRepository(IDapperContext context)
        {
            _context = context;
        }

        public async Task<Employee?> AddAsync(Employee entity)
        {
            using var con = _context.CreateConnection();
            return await con.QuerySingleOrDefaultAsync<Employee>("dbo.AddEmployee", new
            {
                entity.FirstName,
                entity.LastName,
                entity.Email,
                entity.Phone,
                entity.Address,
                entity.DateOfBirth,
                entity.DepartmentId,
                entity.DesignationId
            }, commandType: CommandType.StoredProcedure);
        }

        public Task<Employee> DeleteAsync(int id)
        {
            TODO:throw new NotImplementedException();
        } 

        public async Task<IEnumerable<Employee>> GetAllAsync()
        {
            using (var con = _context.CreateConnection())
            {
                string storeProcedure = "dbo.GetAllEmployee";
                return await con.QueryAsync<Employee>(storeProcedure,commandType:CommandType.StoredProcedure);
            }
        }

        public async Task<Employee?> GetByIdAsync(int id)
        {
            // using (var con = _context.CreateConnection())
            // {
            //     string storeProcedure = "dbo.GetSingleEmployee";
            //     return await con.QuerySingleOrDefaultAsync<Employee>(storeProcedure,id,commandType:CommandType.StoredProcedure);
            // }
            TODO:throw new NotImplementedException();
        }

        public async Task<Employee?> UpdateAsync(Employee entity)
        {
            // using (var con = _context.CreateConnection())
            // {
            //     string storeProcedure = "dbo.UpdateEmployee";
            //     return await con.QueryFirstOrDefaultAsync<Employee>(storeProcedure,entity,commandType:CommandType.StoredProcedure);
            // }
            TODO:throw new NotImplementedException();
        }
    }
}