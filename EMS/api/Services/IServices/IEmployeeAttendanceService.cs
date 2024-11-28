using System.Collections;
using api.Dto.EmployeeAttendance;

namespace api.Services.IServices
{
    public interface IEmployeeAttendanceService
    {
        Task AddAttendanceAsync(int id,EmployeeAttendanceCreateDto attendanceDto);

        Task UpdateAttendanceAsync(int attendanceId, EmployeeAttendanceUpdateDto attendanceDto);

        Task<EmployeeAttendanceDto> GetAttendanceByAttendanceId(int id);


        Task<IEnumerable<EmployeeAttendanceDto>> GetAllAttendanceAsync();

        Task<IEnumerable<EmployeeAttendanceDto>> GetAllUserAttendance(int id);
    }
}
