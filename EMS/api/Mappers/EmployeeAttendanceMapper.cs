using api.Dto.EmployeeAttendance;
using api.Models;

namespace api.Mappers
{
    public static class EmployeeAttendanceMapper
    {

        public static EmployeeAttendanceDto ToEmployeeAttendanceDto(this EmployeeAttendance attendance)
        {
            if (attendance == null)
            {
                throw new ArgumentNullException(nameof(attendance), "EmployeeAttendance cannot be null.");
            }

            return new EmployeeAttendanceDto
            {
                Id = attendance.Id,
                EmployeeId = attendance.EmployeeId,
                CheckInTime = attendance.CheckInTime,
                CheckOutTime = attendance.CheckOutTime,
            };
        }

        public static EmployeeAttendance ToEmployeeAttendance(this EmployeeAttendanceCreateDto dto)
        {
            if (dto == null)
            {
                throw new ArgumentNullException(nameof(dto), "EmployeeAttendanceCreateDto cannot be null.");
            }

            return new EmployeeAttendance
            {
                EmployeeId = dto.EmployeeId,
                CheckInTime = dto.CheckInTime,
            };
        }

        public static EmployeeAttendance ToEmployeeAttendance(this EmployeeAttendanceUpdateDto dto, EmployeeAttendance attendance)
        {
            if (dto == null)
            {
                throw new ArgumentNullException(nameof(dto), "EmployeeAttendanceUpdateDto cannot be null.");
            }
            if (attendance == null)
            {
                throw new ArgumentNullException(nameof(attendance), "EmployeeAttendance cannot be null.");
            }

            attendance.CheckInTime = dto.CheckInTime;
            if (dto.CheckOutTime.HasValue) attendance.CheckOutTime = dto.CheckOutTime.Value;
            return attendance;
        }

        public static EmployeeWithAttendanceDto ToEmployeeWithAttendanceDto(this Employee employee, List<EmployeeAttendance> attendances)
        {
            if (employee == null)
            {
                throw new ArgumentNullException(nameof(employee), "Employee cannot be null.");
            }

            return new EmployeeWithAttendanceDto
            {
                Id = employee.Id,
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                Email = employee.Email,
                Phone = employee.Phone,
                Address = employee.Address,
                DateOfBirth = employee.DateOfBirth,
                IsDeleted = employee.IsDeleted,
                DepartmentId = employee.DepartmentId,
                DesignationId = employee.DesignationId,
                Attendances = attendances?.Select(a => a.ToEmployeeAttendanceDto()).ToList()
            };
        }
    }
}
