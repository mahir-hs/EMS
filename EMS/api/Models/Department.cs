using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace api.Models
{
    public class Department
    {
        public int Id {get;set;}
        [StringLength(20, MinimumLength = 1, ErrorMessage = "The Department must be between 1 and 20 characters.")]
        public required string Dept { get; set; }
        public bool IsDeleted { get; set; }
    }
}