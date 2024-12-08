using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace api.Dto.Employees
{
    public class EmployeeCreateDto
    {
        [StringLength(20, MinimumLength = 1, ErrorMessage = "The Name must be between 1 and 20 characters.")]
        public required string FirstName { get; set; }
        [StringLength(20, MinimumLength = 1, ErrorMessage = "The Name must be between 1 and 20 characters.")]
        public required string LastName { get; set; }
        [EmailAddress]
        public required string Email { get; set; }
        [Phone]
        public required string Phone { get; set; }
        [StringLength(100, MinimumLength = 3)]
        public required string Address { get; set; }
        public required DateTime DateOfBirth { get; set; }
        public int? DepartmentId { get; set; }
        public int? DesignationId { get; set; }
    }
}