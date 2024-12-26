using System.Data;
using api.Data.Contexts;
using api.Dto.EmployeeAttendance;
using api.Helpers;
using api.Models;
using api.Repository.IRepository;
using api.Services.IServices;
using Dapper;
using NpgsqlTypes;

namespace api.Repository
{
    public class EmployeeAttendanceRepository(IFactoryDbContext context, IOperationLogService operationLogService, ILogger<EmployeeAttendanceRepository> logger) : IEmployeeAttendanceRepository
    {

        private readonly IFactoryDbContext _context = context;
        private readonly IOperationLogService _operationLogService = operationLogService;
        private readonly ILogger<EmployeeAttendanceRepository> _logger = logger;
        public async Task<ApiResponse> AddAttendanceAsync(int id,EmployeeAttendance attendance)
        {
            if (attendance == null)
            {
                return new ApiResponse(false,false, "Invalid attendance data.", "400");
            }


            try
            {
                using var sqlCon = _context.SqlConnection();
                var result = await sqlCon.QueryFirstOrDefaultAsync<Employee>(
                "dbo.GetEmployeeById",
                new { Id = id },
                commandType: CommandType.StoredProcedure
                );
                if (result == null)
                {
                    return new ApiResponse(false, false, "Employee not found.", "404");
                }   


                using var pgCon = _context.PgConnection();

                var parameters = new DynamicParameters();
                parameters.Add("p_employee_id", id);
                parameters.Add("p_check_in_time", attendance.CheckInTime);

                await pgCon.ExecuteAsync("SELECT public.addemployeeattendance(@p_employee_id, @p_check_in_time)", parameters);

                try
                {
                    attendance.EmployeeId = id;
                    var log = new OperationLog
                    {
                        OperationType = "Insert",
                        EntityName = "Attendance",
                        EntityId = id,
                        TimeStamp = DateTime.UtcNow,
                        OperationDetails = $"Added Attendance: EmployeeId={id}, CheckInTime={attendance.CheckInTime}"
                    };

                    await _operationLogService.LogOperationAsync(log);
                }
                catch (Exception logEx)
                {
                    _logger.LogError(logEx, "Logging failed for operation on employee attendance {EmployeeId}", id);
                }
                return new ApiResponse(true, true, "Attendance added successfully.", "200");
            }
            catch (Exception ex) {
                _logger.LogError(ex, "An error occurred while adding attendance: {Message}", ex.Message);
                return new ApiResponse(false, false, "An error occurred while adding attendance.", "500");
            }
        }


       

        public async Task<ApiResponse> GetAllAttendanceAsync()
        {
           
            try
            {
                using var pgCon = _context.PgConnection();
                var attendanceRecords = await pgCon.QueryAsync<EmployeeAttendance>("select * from public.getallemployeeattendance()");
                return new ApiResponse(attendanceRecords, true, "Fetched all attendance records successfully.", "200");

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching attendance records: {Message}", ex.Message);
                return new ApiResponse(null, false, "An error occurred while fetching attendance records.", "500");
            }
        }


        public async Task<ApiResponse> GetAttendanceByAttendanceIdAsync(int attendanceId)
        {
            try
            {
                using var pgCon = _context.PgConnection();
                var parametars = new DynamicParameters();
                parametars.Add("p_attendance_id", attendanceId);
                var result = await pgCon.QuerySingleOrDefaultAsync<EmployeeAttendance>("select * from getattendancebyid(@p_attendance_id)", parametars);
                return new ApiResponse(result, true, "Fetched attendance record successfully.", "200");
            }
            catch (Exception ex) {
                _logger.LogError(ex, "An error occurred while fetching attendance records: {Message}", ex.Message);
                return new ApiResponse(null, false, "An error occurred while fetching attendance records.", "500");
            }       

        }

        public async Task<ApiResponse> UpdateAttendanceAsync(int id,EmployeeAttendance attendance)
        {
            
            try
            {
                using var pgCon = _context.PgConnection();
                var parameters = new DynamicParameters();

                parameters.Add("p_attendance_id", id);
                parameters.Add("p_checkin_time", attendance.CheckInTime);
                parameters.Add("p_checkout_time", attendance.CheckOutTime);
                await pgCon.ExecuteAsync("select public.updateemployeeattendance(@p_attendance_id,@p_checkin_time,@p_checkout_time)", parameters);
                try
                {
                    var log = new OperationLog
                    {
                        OperationType = "Update",
                        EntityName = "Attendance",
                        EntityId = id,
                        TimeStamp = DateTime.UtcNow,
                        OperationDetails = $"Updated Attendance: Id={id}, CheckInTime={attendance.CheckInTime}, CheckOutTime={attendance.CheckOutTime}"
                    };
                    await _operationLogService.LogOperationAsync(log);
                }
                catch (Exception logEx)
                {
                    _logger.LogError(logEx, "Logging failed for operation on employee {EmployeeId}", id);
                }
                return new ApiResponse(true, true, "Attendance record updated successfully.", "200");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating attendance record: {Message}", ex.Message);
                return new ApiResponse(false, false, "An error occurred while updating attendance record.", "500");
            }
            
        }

        public async Task<ApiResponse> GetUserAttendance(int id)
        {
            try
            {
                using var sqlCon = _context.SqlConnection();
                var getResult = await sqlCon.QuerySingleOrDefaultAsync<Employee>("dbo.GetEmployeeById", new { id }, commandType: CommandType.StoredProcedure);
                if (getResult == null)
                {
                    return new ApiResponse(null, false, "Employee not found.", "404");
                }
                using var pgCon = _context.PgConnection();
                var x = await pgCon.QueryAsync<EmployeeAttendance>("SELECT * FROM getattendancebyemployeeid(@emp_id);", new { emp_id = id });
                return new ApiResponse(x, true, "Fetched user attendance successfully.", "200");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching user attendance: {Message}", ex.Message);
                return new ApiResponse(null, false, "An error occurred while fetching user attendance.", "500");
            }
            
        }
    }
}
