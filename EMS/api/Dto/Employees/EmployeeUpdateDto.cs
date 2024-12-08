using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace api.Dto.Employees
{
    public class EmployeeUpdateDto
    {
        [StringLength(20, MinimumLength = 1, ErrorMessage = "The Name must be between 1 and 20 characters.")]
        public string? FirstName { get; set; }
        [StringLength(20, MinimumLength = 1, ErrorMessage = "The Name must be between 1 and 20 characters.")]
        public string? LastName { get; set; }
        [EmailAddress]
        public string? Email { get; set; }
        [Phone]
        public string? Phone { get; set; }
        [StringLength(100, MinimumLength = 3)]
        public string? Address { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public int? DepartmentId { get; set; }
        public int? DesignationId { get; set; }
    }
}