using api.Dto.EmployeeAttendance;

namespace api.Services.IServices
{
    public interface IEmployeeAttendanceService
    {
        Task AddAttendanceAsync(EmployeeAttendanceCreateDto attendanceDto);

        Task UpdateAttendanceAsync(int attendanceId, EmployeeAttendanceUpdateDto attendanceDto);


        Task<IEnumerable<EmployeeAttendanceDto>> GetAllAttendanceAsync();


        Task<EmployeeWithAttendanceDto> GetEmployeeWithAttendanceAsync(int employeeId);
        Task <EmployeeAttendanceDto> GetAttendanceAsync(int attendanceID);
    }
}
