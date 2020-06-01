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

        [Range(1,25)]
        public string UserName { get; set; }

        public string Email { get; set; }

        public string Role { get; set; }

    }
}
