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
        public async Task AddAttendanceAsync(int id,EmployeeAttendance attendance)
        {
            if (attendance == null)
                throw new ArgumentNullException(nameof(attendance), "Attendance cannot be null.");

            using var sqlCon = _context.SqlConnection();

            var result = await sqlCon.QuerySingleOrDefaultAsync<Employee>(
                "dbo.GetEmployeeById",
                new { Id = id }, 
                commandType: CommandType.StoredProcedure
            ) ?? throw new InvalidOperationException("Employee not found.");
            using var pgCon = _context.PgConnection();

            var parameters = new DynamicParameters();
            parameters.Add("p_employee_id", id);
            parameters.Add("p_check_in_time", attendance.CheckInTime);

            await pgCon.ExecuteAsync("SELECT public.addemployeeattendance(@p_employee_id, @p_check_in_time)",parameters);
        }


       

        public async Task<IEnumerable<EmployeeAttendance>> GetAllAttendanceAsync()
        {
            using var pgCon = _context.PgConnection();
            var attendanceRecords = await pgCon.QueryAsync<EmployeeAttendance>("select * from public.getallemployeeattendance()");
            return attendanceRecords;
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
        }

        public async Task<IEnumerable<EmployeeAttendance>> GetAllUserAttendance(int id)
        {
            using var sqlCon = _context.SqlConnection();
            var getResult = await sqlCon.QuerySingleOrDefaultAsync<Employee>("dbo.GetEmployeeById", new { id }, commandType: CommandType.StoredProcedure);
            if (getResult != null)
            {
                using var pgCon = _context.PgConnection();
                var x = await pgCon.QueryAsync<EmployeeAttendance>("SELECT * FROM getattendancebyemployeeid(@emp_id);",new { emp_id = id });
                return x;

            }
            else
            {
                return [];
            }
        }
    }
}
