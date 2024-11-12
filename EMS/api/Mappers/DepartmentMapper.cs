using api.Dtos.Department;
using api.Models;

namespace api.Mappers
{
    public static class DepartmentMapper
    {
        public static DepartmentDto ToDepartmentDto(this Department? department)
        {
            return department == null ? throw new ArgumentNullException(nameof(department)) :
            new DepartmentDto
            {
                Id = department.Id,
                Dept = department.Dept
            };
        }

        public static Department ToDepartment(this DepartmentCreateDto dto)
        {
            ArgumentNullException.ThrowIfNull(dto);
            return new Department { Dept = dto.Dept };
        }

        public static Department ToDepartment(this DepartmentUpdateDto dto, Department department)
        {
            ArgumentNullException.ThrowIfNull(dto);
            if (department.Dept != null) department.Dept = dto.Dept;
            return department;
        }
    }
}
