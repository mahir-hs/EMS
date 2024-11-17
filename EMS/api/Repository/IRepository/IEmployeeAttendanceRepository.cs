using api.Dto.EmployeeAttendance;
using api.Models;

namespace api.Repository.IRepository
{
    public interface IEmployeeAttendanceRepository
    {
        Task AddAttendanceAsync(EmployeeAttendance attendance);

        Task<EmployeeAttendance?> GetAttendanceByEmployeeIdAsync(int employeeId);
        Task<EmployeeAttendance?> GetAttendanceByAttendanceIdAsync(int attendanceId);
        Task UpdateAttendanceAsync(int id, EmployeeAttendance attendance);

        Task<IEnumerable<EmployeeAttendance>> GetAllAttendanceAsync();

        
    }
}
