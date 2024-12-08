using System.ComponentModel.DataAnnotations;

namespace api.Dto.Designation
{
    public class DesignationCreateDto
    {
        [StringLength(20, MinimumLength = 1, ErrorMessage = "The role must be between 1 and 20 characters.")]
        public required string Role { get; set; }
    }
}
