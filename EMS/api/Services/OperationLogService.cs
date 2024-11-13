using api.Models;
using api.Repository.IRepository;
using api.Services.IServices;

namespace api.Services
{
    public class OperationLogService(IOperationLogRepository context) : IOperationLogService
    {
        private readonly IOperationLogRepository _context = context;
        public async Task<IEnumerable<OperationLog>> GetAllLogsAsync()
        {
            return await _context.GetAllLogsAsync();
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
