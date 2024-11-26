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

        public async Task AddAttendanceAsync(EmployeeAttendanceCreateDto attendanceDto)
        {
            await _context.AddAttendanceAsync(attendanceDto.ToEmployeeAttendance());
            return;
        }

        
        public async Task UpdateAttendanceAsync(int attendanceId, EmployeeAttendanceUpdateDto attendanceDto)
        {
            var fetch = await _context.GetAttendanceByAttendanceIdAsync(attendanceId);
            await _context.UpdateAttendanceAsync(attendanceId, attendanceDto.ToEmployeeAttendance(fetch!));
            return;
        }

        

        public async Task<EmployeeWithAttendanceDto> GetEmployeeWithAttendanceAsync(int employeeId)
        {
            //var fetch =  await _context.GetAttendanceByEmployeeIdAsync(employeeId);
            //return fetch!.To();
            return  new EmployeeWithAttendanceDto();
        }

        public async Task<IEnumerable<EmployeeAttendanceDto>> GetAllAttendanceAsync()
        {
            var employeeAttendances = await _context.GetAllAttendanceAsync();

            // Map EmployeeAttendance to EmployeeAttendanceDto
            return employeeAttendances.Select(ea => new EmployeeAttendanceDto
            {
                Id = ea.Id,
                EmployeeId = ea.EmployeeId,
                CheckInTime = ea.CheckInTime,
                CheckOutTime = ea.CheckOutTime

            });

        }

        public async Task<EmployeeAttendanceDto> GetAttendanceAsync(int attendanceID)
        {
            var fetch =  await _context.GetAttendanceByAttendanceIdAsync(attendanceID);
            return fetch.ToEmployeeAttendanceDto();
        }
    }
}
