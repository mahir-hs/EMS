using api.Data.Contexts;
using api.Models;
using api.Repository.IRepository;
using MongoDB.Driver;

namespace api.Repository
{
    public class OperationLogRepository(IMongoContext context) : IOperationLogRepository
    {
        private readonly IMongoCollection<OperationLog> _context = context.Database.GetCollection<OperationLog>("OperationLogs");
        public async Task<IEnumerable<OperationLog>> GetAllLogsAsync()
        {
            return await _context.Find(_=>true).ToListAsync();
        }

        public async Task<List<OperationLog>> GetLogsForEmployeeAsync(int employeeId)
        {
            var filter = Builders<OperationLog>.Filter.And(
             Builders<OperationLog>.Filter.Eq(log => log.EntityName, "Employee"),  
             Builders<OperationLog>.Filter.Eq(log => log.EntityId, employeeId)      
             );


            return await _context.Find(filter).ToListAsync();
        }

        public async Task LogOperationAsync(OperationLog log)
        {
            await _context.InsertOneAsync(log);
        }
    }
}
