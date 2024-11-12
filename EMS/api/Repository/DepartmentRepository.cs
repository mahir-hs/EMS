using System.Data;
using api.Data.Contexts;
using api.Models;
using api.Repository.IRepository;
using Dapper;
using static Dapper.SqlMapper;

namespace api.Repository
{
    public class DepartmentRepository(IDapperContext context) : IDepartmentRepository
    {
        private readonly IDapperContext _context = context;
        public async Task<Department?> AddAsync(Department entity)
        {
            using var con = _context.CreateConnection();
            return await con.QueryFirstOrDefaultAsync<Department>("dbo.AddDepartment", new {entity.Dept},commandType:CommandType.StoredProcedure);
        }

        public async Task<Department?> DeleteAsync(int id)
        {
            using var con = _context.CreateConnection();
            return await con.QueryFirstOrDefaultAsync<Department>("dbo.DeleteDepartment", new { id }, commandType: CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<Department?>> GetAllAsync()
        {
            using var con = _context.CreateConnection();
            return await con.QueryAsync<Department>("dbo.GetAllDepartments", commandType: CommandType.StoredProcedure);
        }

        public async Task<Department?> GetByIdAsync(int id)
        {
            using var con = _context.CreateConnection();
            return await con.QueryFirstOrDefaultAsync<Department>("dbo.GetDepartmentById", new { id }, commandType: CommandType.StoredProcedure);
        }

        public async Task<Department?> UpdateAsync(Department entity)
        {
            using var con = _context.CreateConnection();
            return await con.QueryFirstOrDefaultAsync<Department>("dbo.UpdateDepartment", new
            {
                entity.Id,
                entity.Dept
            },commandType:CommandType.StoredProcedure);
        }
    }
}
