using api.Models;
using api.Repository.IRepository;
using api.Services.IServices;

namespace api.Services
{
    public class OperationLogService(IOperationLogRepository context, ILogger<OperationLogService> logger) : IOperationLogService
    {
        private readonly IOperationLogRepository _context = context;
        private readonly ILogger<OperationLogService> _logger = logger;
        public async Task<IEnumerable<OperationLog>> GetAllLogsAsync()
        {
            return await _context.GetAllLogsAsync();
        }

        public Task<List<OperationLog>> GetLogsForAttendanceAsync(int employeeId)
        {
            return _context.GetLogsForAttendanceAsync(employeeId);
        }

        public async Task<List<OperationLog>> GetLogsForEmployeeAsync(int employeeId)
        {
            return await _context.GetLogsForEmployeeAsync(employeeId);
        }

        public async Task LogOperationAsync(OperationLog log)
        {
            await _context.LogOperationAsync(log);
        }

    }
}
