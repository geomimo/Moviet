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
using Moviet.Services.Interfaces;

namespace Moviet.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class ListController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IMapper _mapper;
        private readonly IBanService _banService;

        public ListController(UserManager<IdentityUser> userManager, IMapper mapper, IBanService banService)
        {
            _userManager = userManager;
            _mapper = mapper;
            _banService = banService;
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

        public IActionResult BanUser(string id)
        {
            var banned = _banService.BanUser(id);
            if (!banned)
            {
                return NotFound();
            }

            return RedirectToAction(nameof(AllUsers));

        }

    }
}