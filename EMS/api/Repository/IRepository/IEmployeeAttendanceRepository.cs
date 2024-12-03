using api.Dto.EmployeeAttendance;
using api.Models;

namespace api.Repository.IRepository
{
    public interface IEmployeeAttendanceRepository
    {
        Task<ApiResponse> AddAttendanceAsync(int id,EmployeeAttendance attendance);

        Task<ApiResponse> GetAttendanceByAttendanceIdAsync(int attendanceId);
        Task<ApiResponse> UpdateAttendanceAsync(int id, EmployeeAttendance attendance);

        Task<ApiResponse> GetAllAttendanceAsync();
        Task<ApiResponse> GetAllUserAttendance(int id);


    }
}
