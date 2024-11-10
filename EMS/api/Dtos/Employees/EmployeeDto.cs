using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using api.Models;

namespace api.Dtos.Employees
{
    public class EmployeeDto
    {
        public int Id {get;set;}
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        [EmailAddress(ErrorMessage ="Invalid email Address")]
        public required string Email { get; set; }

        public required string Phone { get; set; }
        public required string Address { get; set; }

        public required DateTime DateOfBirth {get; set;}


         public int? DepartmentId { get; set; }
         public int? DesignationId { get; set; }


    }
}