using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moviet.Models;
using Moviet.Services;
using Moviet.Services.Interfaces;

namespace Moviet.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IRoleService _roleService;
        private readonly IMapper _mapper;

        public AccountController(UserManager<IdentityUser> userManager, IRoleService roleService, IMapper mapper)
        {
            _userManager = userManager;
            _roleService = roleService;
            _mapper = mapper;
        }

        public IActionResult Index()
        {
            IdentityUser user = _userManager.GetUserAsync(User).Result;
            IdentityUserVM model = _mapper.Map<IdentityUserVM>(user);
            var role = _userManager.GetRolesAsync(user).Result;
            return View(model);
        }

        [Authorize(Roles = "Rater")]
        public IActionResult UpgradeRole()
        {
            _roleService.UpgradeToContentManager(User);


            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "ContentManager")]
        public IActionResult DowngradeRole()
        {
            _roleService.DowngradeToRater(User);

            return RedirectToAction(nameof(Index));
        }
    }
}