using Microsoft.AspNetCore.Identity;
using Moviet.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Moviet.Services
{
    public class BanService : IBanService
    {
        private readonly UserManager<IdentityUser> _userManager;

        public BanService(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        public bool BanPost(int postId)
        {
            throw new NotImplementedException();
        }

        public bool BanUser(string userId)
        {
            IdentityUser user = _userManager.FindByIdAsync(userId).Result;
            return _userManager.DeleteAsync(user).Result.Succeeded;
        }
    }
}
