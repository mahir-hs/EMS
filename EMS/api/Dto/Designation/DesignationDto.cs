using System.ComponentModel.DataAnnotations;

namespace api.Dto.Designation
{
    public class DesignationDto
    {
        public int Id { get; set; }
        public required string Role { get; set; }
    }
}
