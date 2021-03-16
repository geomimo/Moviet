using Microsoft.AspNetCore.Identity;
using Moviet.Data;
using Moviet.Services.Interfaces;
using System.Security.Claims;

namespace Moviet.Services
{
    public class RoleService : IRoleService
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public RoleService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public void UpgradeToContentManager(ClaimsPrincipal principal)
        {
            var user = _userManager.GetUserAsync(principal).Result;
            _userManager.RemoveFromRoleAsync(user, Roles.Rater).Wait();
            _userManager.AddToRoleAsync(user, Roles.ContentManager).Wait();
        }

        public void DowngradeToRater(ClaimsPrincipal principal)
        {
            var user = _userManager.GetUserAsync(principal).Result;
            _userManager.RemoveFromRoleAsync(user, Roles.ContentManager).Wait();
            _userManager.AddToRoleAsync(user, Roles.Rater).Wait();
        }
    }
}
