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

        public async Task<Employee?> DeleteAsync(int id)
        {
            using var con = _context.CreateConnection();
            return await con.QueryFirstOrDefaultAsync<Employee>("dbo.DeleteEmployee", new
            {
                id,
            }, commandType: CommandType.StoredProcedure);
        } 

        public async Task<IEnumerable<Employee?>> GetAllAsync()
        {
            using var con = _context.CreateConnection();
            return await con.QueryAsync<Employee>("dbo.GetAllEmployees", commandType: CommandType.StoredProcedure);
        }

        public async Task<Employee?> GetByIdAsync(int Id)
        {
            using var con = _context.CreateConnection();
            return await con.QuerySingleOrDefaultAsync<Employee>("dbo.GetEmployeeById", new { Id }, commandType: CommandType.StoredProcedure);

        }

        public async Task<Employee?> UpdateAsync(Employee entity)
        {
            using var con = _context.CreateConnection();
            return await con.QueryFirstOrDefaultAsync<Employee>("dbo.UpdateEmployee", new
            {
                entity.Id,
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
    }
}