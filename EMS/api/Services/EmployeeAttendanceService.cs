using api.Dto.EmployeeAttendance;
using api.Mappers;
using api.Repository.IRepository;
using api.Services.IServices;
using Microsoft.AspNetCore.Http.HttpResults;

namespace api.Services
{
    public class EmployeeAttendanceService(IEmployeeAttendanceRepository context) : IEmployeeAttendanceService
    {
        private readonly IEmployeeAttendanceRepository _context = context;

        public async Task AddAttendanceAsync(int id,EmployeeAttendanceCreateDto attendanceDto)
        {
            await _context.AddAttendanceAsync(id,attendanceDto.ToEmployeeAttendance());
            return;
        }

        
        public async Task UpdateAttendanceAsync(int attendanceId, EmployeeAttendanceUpdateDto attendanceDto)
        {
            var fetch = await _context.GetAttendanceByAttendanceIdAsync(attendanceId);
            await _context.UpdateAttendanceAsync(attendanceId, attendanceDto.ToEmployeeAttendance(fetch!));
            return;
        }

        

        public async Task<IEnumerable<EmployeeAttendanceDto>> GetAllAttendanceAsync()
        {
            var employeeAttendances = await _context.GetAllAttendanceAsync();

            return employeeAttendances.Select(ea => new EmployeeAttendanceDto
            {
                Id = ea.Id,
                EmployeeId = ea.EmployeeId,
                CheckInTime = ea.CheckInTime,
                CheckOutTime = ea.CheckOutTime

            });

        }

       

        public async Task<IEnumerable<EmployeeAttendanceDto>> GetAllUserAttendance(int id)
        {
            var employeeAttendances = await _context.GetAllUserAttendance(id);

            return employeeAttendances.Select(ea => new EmployeeAttendanceDto
            {
                Id = ea.Id,
                EmployeeId = ea.EmployeeId,
                CheckInTime = ea.CheckInTime,
                CheckOutTime = ea.CheckOutTime

            });
        }

        public async Task<EmployeeAttendanceDto> GetAttendanceByAttendanceId(int id)
        {
            var employeeAttendance = await _context.GetAttendanceByAttendanceIdAsync(id);
            return employeeAttendance.ToEmployeeAttendanceDto();

        }
    }
}
