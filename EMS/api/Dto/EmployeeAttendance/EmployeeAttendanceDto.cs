namespace api.Dto.EmployeeAttendance
{
    public class EmployeeAttendanceDto
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public DateTime CheckInTime { get; set; }
        public DateTime? CheckOutTime { get; set; }
    
    }
}
