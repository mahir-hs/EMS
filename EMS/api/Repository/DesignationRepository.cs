using System.Data;
using api.Data.Contexts;
using api.Models;
using api.Repository.IRepository;
using Dapper;

namespace api.Repository
{
    public class DesignationRepository(IDapperContext context) : IDesignationRepository
    {
        private readonly IDapperContext _context = context;

        public async Task<Designation?> AddAsync(Designation entity)
        {
            using var con = _context.CreateConnection();
            return await con.QuerySingleOrDefaultAsync<Designation>("dbo.AddDesignation", new {entity.Role}, commandType: CommandType.StoredProcedure);
        }

        public async Task<Designation?> DeleteAsync(int id)
        {
            using var con = _context.CreateConnection();
            return await con.QueryFirstOrDefaultAsync<Designation>("DeleteDesignation", new { id }, commandType: CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<Designation?>> GetAllAsync()
        {
            using var con = _context.CreateConnection();
            return await con.QueryAsync<Designation>("dbo.GetAllDesignations", commandType: CommandType.StoredProcedure);
        }

        public async Task<Designation?> GetByIdAsync(int id)
        {
            using var con = _context.CreateConnection();
            return await con.QueryFirstOrDefaultAsync<Designation>("GetDesignationById", new {id},commandType: CommandType.StoredProcedure);
        }

        public async Task<Designation?> UpdateAsync(Designation entity)
        {
            using var con = _context.CreateConnection();
            return await con.QueryFirstOrDefaultAsync<Designation>("dbo.UpdateDesignation", new
            {
                entity.Id,
                entity.Role
            }, commandType: CommandType.StoredProcedure);
        }
    }
}
