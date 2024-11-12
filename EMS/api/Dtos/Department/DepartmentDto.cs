using System.ComponentModel.DataAnnotations;

namespace api.Dtos.Department
{
    public class DepartmentDto
    {
        public int Id { get; set; }
        public required string Dept { get; set; }
    }
}
