using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace api.Models
{
    public class Designation
    {
        public int Id {get;set;}
        [StringLength(20, MinimumLength = 1, ErrorMessage = "The role must be between 1 and 20 characters.")]
        public required string Role { get; set; }
        public bool IsDeleted { get; set; }
    }
}