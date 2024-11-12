using System.ComponentModel.DataAnnotations;

namespace api.Dtos.Designation
{
    public class DesignationDto
    {
        public int Id { get; set; }
        public required string Role { get; set; }
    }
}
