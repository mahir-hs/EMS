using api.Dto.EmployeeAttendance;
using api.Models;

namespace api.Repository.IRepository
{
    public interface IEmployeeAttendanceRepository
    {
        Task AddAttendanceAsync(int id,EmployeeAttendance attendance);

        Task<EmployeeAttendance?> GetAttendanceByAttendanceIdAsync(int attendanceId);
        Task UpdateAttendanceAsync(int id, EmployeeAttendance attendance);

        Task<IEnumerable<EmployeeAttendance>> GetAllAttendanceAsync();
        Task<IEnumerable<EmployeeAttendance>> GetAllUserAttendance(int employeeId);


    }
}
