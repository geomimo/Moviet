using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Moviet.Models
{
    public class IdentityUserVM
    {
        public int Id { get; set; }

        [Display(Name="Username: ")]
        public string Username { get; set; }

        [Display(Name = "Email: ")]
        public string Email { get; set; }
    }
}
