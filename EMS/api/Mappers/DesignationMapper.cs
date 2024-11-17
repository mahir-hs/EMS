using api.Dto.Designation;
using api.Models;

namespace api.Mappers
{
    public static class DesignationMapper
    {
        public static DesignationDto ToDesignationDto(this Designation? designation)
        {
            return designation == null ? throw new ArgumentNullException(nameof(designation)) : 
            new DesignationDto
            {
                Id = designation.Id,
                Role = designation.Role
            };
        }

        public static Designation ToDesignation(this DesignationCreateDto dto)
        {
            ArgumentNullException.ThrowIfNull(dto);
            return new Designation { Role = dto.Role };
        }

        public static Designation ToDesignation(this DesignationUpdateDto dto, Designation designation)
        {
            ArgumentNullException.ThrowIfNull(dto);
            if(designation.Role != null) designation.Role = dto.Role;
            return designation;
        }
    }
}
