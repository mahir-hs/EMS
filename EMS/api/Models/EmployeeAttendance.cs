namespace api.Models
{
    public class EmployeeAttendance
    {
        public int Id { get; set; }                
        public int EmployeeId { get; set; }        
        public DateTime CheckInTime { get; set; }  
        public DateTime? CheckOutTime { get; set; } 
    }
}
