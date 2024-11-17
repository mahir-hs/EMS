namespace api.Dto.EmployeeAttendance
{
    public class EmployeeWithAttendanceDto
    {
        public int Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public DateTime DateOfBirth { get; set; }
        public bool IsDeleted { get; set; }
        public int? DepartmentId { get; set; }
        public int? DesignationId { get; set; }

        public List<EmployeeAttendanceDto>? Attendances { get; set; }
    }
}
