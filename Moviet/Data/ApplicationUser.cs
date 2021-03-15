using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Moviet.Data
{
    public class ApplicationUser : IdentityUser
    {
        public bool IsNew { get; set; }
    }
}
