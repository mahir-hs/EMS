using api.Data.Contexts;
using api.Models;
using api.Repository.IRepository;
using MongoDB.Driver;

namespace api.Repository
{
    public class OperationLogRepository(IFactoryDbContext context,ILogger<OperationLogRepository> logger) : IOperationLogRepository
    {
        private readonly IMongoCollection<OperationLog> _context = context.MongoConnection.GetCollection<OperationLog>("OperationLogs");
        private readonly ILogger<OperationLogRepository> _logger= logger;
        public async Task<IEnumerable<OperationLog>> GetAllLogsAsync()
        {
            try
            {
                return await _context.Find(_ => true).ToListAsync();
            }
            catch(MongoException ex)
            {
                _logger.LogError(ex, "An error occurred while fetching logs: {Message}", ex.Message);
                return Enumerable.Empty<OperationLog>();
            }
        }

        public async Task<List<OperationLog>> GetLogsForAttendanceAsync(int employeeId)
        {
            try
            {
                var filter = Builders<OperationLog>.Filter.And(
             Builders<OperationLog>.Filter.Eq(log => log.EntityName, "Attendance"),
             Builders<OperationLog>.Filter.Eq(log => log.EntityId, employeeId)
             );


                return await _context.Find(filter).ToListAsync();
            }
            catch (MongoException ex)
            {
                _logger.LogError(ex, "An error occurred while fetching logs: {Message}", ex.Message);
                return Enumerable.Empty<OperationLog>().ToList();
            }
        }

        public async Task<List<OperationLog>> GetLogsForEmployeeAsync(int employeeId)
        {
            try
            {
                var filter = Builders<OperationLog>.Filter.And(
             Builders<OperationLog>.Filter.Eq(log => log.EntityName, "Employee"),
             Builders<OperationLog>.Filter.Eq(log => log.EntityId, employeeId)
             );


                return await _context.Find(filter).ToListAsync();
            }
            catch (MongoException ex) {
                _logger.LogError(ex, "An error occurred while fetching logs: {Message}", ex.Message);
                return Enumerable.Empty<OperationLog>().ToList();
            }
        }

        

        public async Task LogOperationAsync(OperationLog log)
        {
            try
            {
                await _context.InsertOneAsync(log);
            }
            catch (MongoException ex) {
                _logger.LogError(ex, "An error occurred while logging operation: {Message}", ex.Message);
            }
        }
    }
}
