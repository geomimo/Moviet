using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Moviet.Models
{
    public class IdentityUserVM
    {
        public string Id { get; set; }

        [Display(Name="Username: ")]
        public string UserName { get; set; }

        [Display(Name = "Email: ")]
        public string Email { get; set; }

        public string Role { get; set; }

    }
}
