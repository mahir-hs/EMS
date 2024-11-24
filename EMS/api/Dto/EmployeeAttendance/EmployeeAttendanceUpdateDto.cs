namespace api.Dto.EmployeeAttendance
{
    public class EmployeeAttendanceUpdateDto
    {
        public DateTime CheckInTime { get; set; }
        public DateTime? CheckOutTime { get; set; }      
    }
}
