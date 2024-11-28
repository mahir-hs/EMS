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
    }
}
