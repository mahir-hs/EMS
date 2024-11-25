using System.Data;
using api.Data.Contexts;
using api.Dto.EmployeeAttendance;
using api.Models;
using api.Repository.IRepository;
using Dapper;
using NpgsqlTypes;

namespace api.Repository
{
    public class EmployeeAttendanceRepository(IFactoryDbContext context) : IEmployeeAttendanceRepository
    {

        private readonly IFactoryDbContext _context = context;
        public async Task AddAttendanceAsync(EmployeeAttendance attendance)
        {
            if (attendance == null)
                throw new ArgumentNullException(nameof(attendance), "Attendance cannot be null.");

            using var sqlCon = _context.SqlConnection();

            var getResult = await sqlCon.QuerySingleOrDefaultAsync<Employee>(
                "dbo.GetEmployeeById",
                new { Id = attendance.EmployeeId }, 
                commandType: CommandType.StoredProcedure
            ) ?? throw new Exception($"Employee with ID {attendance.EmployeeId} does not exist.");
            using var pgCon = _context.PgConnection();

            var parameters = new DynamicParameters();
            parameters.Add("p_employee_id", attendance.EmployeeId);
            parameters.Add("p_check_in_time", attendance.CheckInTime);

            await pgCon.ExecuteAsync("SELECT public.addemployeeattendance(@p_employee_id, @p_check_in_time)",parameters);
        }


       

        public async Task<IEnumerable<EmployeeAttendance>> GetAllAttendanceAsync()
        {
            using var pgCon = _context.PgConnection();
            var attendanceRecords = await pgCon.QueryAsync<EmployeeAttendance>("select * from public.getallemployeeattendance()");

            foreach (var record in attendanceRecords)
            {
                Console.WriteLine($"Employee ID: {record.EmployeeId}, Check-in Time: {record.CheckInTime}");
            }

            return attendanceRecords;

        }

        public async Task<EmployeeAttendance?> GetAttendanceByEmployeeIdAsync(int employeeId)
        {
            using var sqlCon = _context.SqlConnection();
            var getResult = await sqlCon.QuerySingleOrDefaultAsync<Employee>("dbo.GetEmployeeById", new { employeeId }, commandType: CommandType.StoredProcedure);
            if (getResult != null)
            {
                using var pgCon = _context.PgConnection();
                return await pgCon.QueryFirstOrDefaultAsync<EmployeeAttendance>("GetAttendanceByEmployeeId", new { employeeId }, commandType: CommandType.StoredProcedure);

            }
            return null;        
        }

        public async Task<EmployeeAttendance?> GetAttendanceByAttendanceIdAsync(int attendanceId)
        {
            
            using var pgCon = _context.PgConnection();
            var parametars = new DynamicParameters();
            parametars.Add("p_attendance_id",attendanceId);
            return await pgCon.QueryFirstOrDefaultAsync<EmployeeAttendance>("select * from getattendancebyid(@p_attendance_id)", parametars);

        }

        public async Task UpdateAttendanceAsync(int id,EmployeeAttendance attendance)
        {
           
            using var pgCon = _context.PgConnection();
            var parametars = new DynamicParameters();
              
            parametars.Add("p_attendance_id",id);
            parametars.Add("p_checkin_time", attendance.CheckInTime);
            parametars.Add("p_checkout_time", attendance.CheckOutTime);
            await pgCon.ExecuteAsync("select public.updateemployeeattendance(@p_attendance_id,@p_checkin_time,@p_checkout_time)",parametars);
            return;
            
        }
    }
}
