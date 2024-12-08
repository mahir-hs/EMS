using System.ComponentModel.DataAnnotations;

namespace api.Dto.Department
{
    public class DepartmentDto
    {
        public int Id { get; set; }
        [StringLength(20, MinimumLength = 1, ErrorMessage = "The department name must be between 1 and 20 characters.")]
        public required string Dept { get; set; }
    }
}
