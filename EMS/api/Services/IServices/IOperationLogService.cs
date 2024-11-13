using api.Models;

namespace api.Services.IServices
{
    public interface IOperationLogService
    {
        Task LogOperationAsync(OperationLog log);
        Task<IEnumerable<OperationLog>> GetAllLogsAsync();
        Task<List<OperationLog>> GetLogsForEmployeeAsync(int employeeId);
    }
}
