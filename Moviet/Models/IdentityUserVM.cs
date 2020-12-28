using System;
using System.ComponentModel.DataAnnotations;

namespace Moviet.Models
{
    public class IdentityUserVM
    {
        public string Id { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }

        public string Role { get; set; }

    }
}
