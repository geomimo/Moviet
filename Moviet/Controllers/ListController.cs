using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moviet.Models;

namespace Moviet.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class ListController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IMapper _mapper;

        public ListController(UserManager<IdentityUser> userManager, IMapper mapper)
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
            allUsers.AddRange(contentManager);
            allUsers.Sort((p, q) => p.UserName.CompareTo(q.UserName));

            var model = _mapper.Map<List<IdentityUserVM>>(allUsers);

            return View(model);
        }   

        public IActionResult AllPosts()
        {
            return View();
        }
    }
}