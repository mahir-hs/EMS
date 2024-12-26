using System.Collections;
using api.Dto.EmployeeAttendance;
using api.Models;

namespace api.Services.IServices
{
    public interface IEmployeeAttendanceService
    {
        Task<ApiResponse> AddAttendanceAsync(int id,EmployeeAttendanceCreateDto attendanceDto);

        Task<ApiResponse> UpdateAttendanceAsync(int attendanceId, EmployeeAttendanceUpdateDto attendanceDto);

        Task<ApiResponse> GetAttendanceByAttendanceId(int id);


        Task<ApiResponse> GetAllAttendanceAsync();

        Task<ApiResponse> GetUserAttendance(int id);
    }
}
