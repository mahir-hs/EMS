using api.Dto.EmployeeAttendance;
using api.Mappers;
using api.Models;
using api.Repository.IRepository;
using api.Services.IServices;
using Microsoft.AspNetCore.Http.HttpResults;

namespace api.Services
{
    public class EmployeeAttendanceService(IEmployeeAttendanceRepository context, ILogger<EmployeeAttendanceService> logger) : IEmployeeAttendanceService
    {
        private readonly IEmployeeAttendanceRepository _context = context;
        private readonly ILogger<EmployeeAttendanceService> _logger = logger;

        public async Task<ApiResponse> AddAttendanceAsync(int id,EmployeeAttendanceCreateDto attendanceDto)
        {
            try
            {
                await _context.AddAttendanceAsync(id, attendanceDto.ToEmployeeAttendance());
                return new ApiResponse(true, true, "Attendance added successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while adding attendance for Employee ID: {Id}", id);
                return new ApiResponse(false, false, "Failed to add attendance.", "500");
            }
        }

        
        public async Task<ApiResponse> UpdateAttendanceAsync(int attendanceId, EmployeeAttendanceUpdateDto attendanceDto)
        {
            try
            {
                var fetch = await _context.GetAttendanceByAttendanceIdAsync(attendanceId);
                if (!fetch.Success)
                {
                    return new ApiResponse(false, false, "Attendance not found.", "404");
                }

                var data = fetch.Result as EmployeeAttendance;
                
                await _context.UpdateAttendanceAsync(attendanceId, attendanceDto.ToEmployeeAttendance(data!));
                return new ApiResponse(true, true, "Attendance updated successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while updating attendance ID: {AttendanceId}", attendanceId);
                return new ApiResponse(false, false, "Failed to update attendance.", "500");
            }
            
        }

        public async Task<ApiResponse> GetAllAttendanceAsync()
        {

            try
            {
                var employeeAttendances = await _context.GetAllAttendanceAsync();
                var result = employeeAttendances.Result as IEnumerable<EmployeeAttendance>;
                var resutlDto  = result?.Select(x => x.ToEmployeeAttendanceDto());
                return new ApiResponse(resutlDto, true, "Attendances retrieved successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while fetching all attendances.");
                return new ApiResponse(null, false, "Failed to fetch attendances.", "500");
            }
        }

       

        public async Task<ApiResponse> GetAllUserAttendance(int id)
        {
            try
            {
                var employeeAttendances = await _context.GetAllUserAttendance(id);
                var result = employeeAttendances.Result as IEnumerable<EmployeeAttendance>;
                var resutlDto = result?.Select(x => x.ToEmployeeAttendanceDto());
                return new ApiResponse(resutlDto, true, "User attendances retrieved successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while fetching attendances for Employee ID: {Id}", id);
                return new ApiResponse(null, false, "Failed to fetch user attendances.", "500");
            }
            
        }

        public async Task<ApiResponse> GetAttendanceByAttendanceId(int id)
        {
            try
            {
                var employeeAttendance = await _context.GetAttendanceByAttendanceIdAsync(id);
                if (!employeeAttendance.Success)
                {
                    return new ApiResponse(null, false, "Attendance not found.", "404");
                }

                var data = employeeAttendance.Result as EmployeeAttendance;

                return new ApiResponse(data!.ToEmployeeAttendanceDto(), true, "Attendance retrieved successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while fetching attendance ID: {Id}", id);
                return new ApiResponse(null, false, "Failed to fetch attendance.", "500");
            }

        }
    }
}
