using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace api.Models
{
    public class Employee
    {
        public int Id {get;set;}
        [StringLength(20, MinimumLength = 1, ErrorMessage = "The Name must be between 1 and 20 characters.")]
        public required string FirstName { get; set; }
        [StringLength(20, MinimumLength = 1, ErrorMessage = "The Name must be between 1 and 20 characters.")]
        public required string LastName { get; set; }
        [EmailAddress(ErrorMessage ="Invalid email Address")]
        public required string Email { get; set; }
        [Phone(ErrorMessage = "Invalid phone number")]
        public required string Phone { get; set; }
        [StringLength(100, MinimumLength = 3, ErrorMessage = "The Address must be between 3 to 100 characters.")]
        public required string Address { get; set; }

        public required DateTime DateOfBirth {get; set;}

        public bool IsDeleted { get; set; }

         public int? DepartmentId { get; set; }
         public int? DesignationId { get; set; }

         public Department? Department { get; set; }
         public Designation? Designation { get; set; }
        
    }
}