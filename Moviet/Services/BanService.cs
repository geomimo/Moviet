using Microsoft.AspNetCore.Identity;
using Moviet.Contracts;
using Moviet.Data;
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
        private readonly IPostRepository _postrepo;

        public BanService(UserManager<IdentityUser> userManager, IPostRepository postrepo)
        {
            _userManager = userManager;
            _postrepo = postrepo;
        }

        public bool BanPost(int postId)
        {
            Post post = _postrepo.FindById(postId);
            return _postrepo.Delete(post);
        }

        public bool BanUser(string userId)
        {
            IdentityUser user = _userManager.FindByIdAsync(userId).Result;
            return _userManager.DeleteAsync(user).Result.Succeeded;
        }
    }
}
