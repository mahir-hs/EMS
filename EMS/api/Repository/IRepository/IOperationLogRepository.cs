using api.Models;

namespace api.Repository.IRepository
{
    public interface IOperationLogRepository
    {
        Task LogOperationAsync(OperationLog log);
        Task<IEnumerable<OperationLog>> GetAllLogsAsync();
        Task<List<OperationLog>> GetLogsForEmployeeAsync(int employeeId);
        Task<List<OperationLog>> GetLogsForAttendanceAsync(int employeeId);
    }
}
