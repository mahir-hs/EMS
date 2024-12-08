using System.ComponentModel.DataAnnotations;

namespace api.Dto.Department
{
    public class DepartmentCreateDto
    {
        [StringLength(20, MinimumLength = 1, ErrorMessage = "The Department must be between 1 and 20 characters.")]

        public required string Dept { get; set; }
    }
}
