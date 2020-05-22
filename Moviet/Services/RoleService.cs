using Microsoft.AspNetCore.Identity;
using Moviet.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace Moviet.Services
{
    public class RoleService : IRoleService
    {
        private readonly UserManager<IdentityUser> _userManager;

        public RoleService(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        public void UpgradeToContentManager(ClaimsPrincipal principal)
        {
            IdentityUser user = _userManager.GetUserAsync(principal).Result;
            _userManager.RemoveFromRoleAsync(user, Roles.Rater).Wait();
            _userManager.AddToRoleAsync(user, Roles.ContentManager).Wait();
        }

        public void DowngradeToRater(ClaimsPrincipal principal)
        {
            IdentityUser user = _userManager.GetUserAsync(principal).Result;
            _userManager.RemoveFromRoleAsync(user, Roles.ContentManager).Wait();
            _userManager.AddToRoleAsync(user, Roles.Rater).Wait();
        }
    }
}
