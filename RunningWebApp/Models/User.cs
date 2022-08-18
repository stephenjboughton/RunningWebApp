using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RunningWebApp.Models
{
    public class User
    {
        public int Id { get; set; }

        public string FName { get; set; }

        public string LName { get; set; }
        
        public string EmailAddress { get; set; }

        public string Salt { get; set; }

        [Required]
        [MinLength(6)]
        public string Password { get; set; }

        public string Role { get; set; }
    }
}
