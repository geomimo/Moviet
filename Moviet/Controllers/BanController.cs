using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Moviet.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class BanController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IMapper _mapper;

        public BanController(UserManager<IdentityUser> userManager, IMapper mapper)
        {
            _userManager = userManager;
            _mapper = mapper;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult AllUsers()
        {
            var raters = _userManager.GetUsersInRoleAsync(Roles.Rater).Result;
            var contentManager = _userManager.GetUsersInRoleAsync(Roles.ContentManager).Result;

            var allUsers = new List<IdentityUser>();
            allUsers.AddRange(raters);
            allUsers.AddRange(raters);
            allUsers.Sort((p, q) => p.UserName.CompareTo(q.UserName));

            var model = _mapper.Map<List<IdentityUser>>(allUsers);

            return View(model);
        }

        [HttpPost]
        public IActionResult BanUser(string id)
        {
            return View();
        }

        public IActionResult AllPosts()
        {
            return View();
        }

        [HttpPost]
        public IActionResult BanPost(string id)
        {
            return View();
        }
    }
}