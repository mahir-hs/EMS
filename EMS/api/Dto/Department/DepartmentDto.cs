using System.ComponentModel.DataAnnotations;

namespace api.Dto.Department
{
    public class DepartmentDto
    {
        public int Id { get; set; }
        public required string Dept { get; set; }
    }
}
