using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moviet.Data;
using Moviet.Models;
using Moviet.Services.Interfaces;
using System.Threading.Tasks;

namespace Moviet.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IRoleService _roleService;
        private readonly IMapper _mapper;

        public AccountController(UserManager<ApplicationUser> userManager,
                                 SignInManager<ApplicationUser> signInManager,
                                 IRoleService roleService,
                                 IMapper mapper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleService = roleService;
            _mapper = mapper;
        }

        public IActionResult Index()
        {
            IdentityUser user = _userManager.GetUserAsync(User).Result;
            IdentityUserVM model = _mapper.Map<IdentityUserVM>(user);
            return View(model);
        }

        [Authorize(Roles = "ContentManager,Rater")]
        public IActionResult ChangeUsername(IdentityUser model)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction(nameof(Index));
            }

            var user = _userManager.GetUserAsync(User).Result;
            user.UserName = model.UserName;
            var result = _userManager.UpdateAsync(user).Result;
            if (result.Succeeded)
            {
                return RedirectToAction(nameof(Index));

            }
            return NotFound();

        }

        [Authorize(Roles = "ContentManager,Rater")]
        public IActionResult ChangeEmail(IdentityUser model)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction(nameof(Index));
            }

            var user = _userManager.GetUserAsync(User).Result;
            user.Email = model.Email;
            var result = _userManager.UpdateAsync(user).Result;
            if (result.Succeeded)
            {
                return RedirectToAction(nameof(Index));

            }
            return NotFound();

        }


        [Authorize(Roles = "Rater")]
        public async Task<IActionResult> UpgradeRole()
        {
            _roleService.UpgradeToContentManager(User);
            // Refresh claims to update role.
            await _signInManager.RefreshSignInAsync(await _userManager.GetUserAsync(User));

            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "ContentManager")]
        public async Task<IActionResult> DowngradeRole()
        {
            _roleService.DowngradeToRater(User);
            // Refresh claims to update role.
            await _signInManager.RefreshSignInAsync(await _userManager.GetUserAsync(User));

            return RedirectToAction(nameof(Index));
        }
    }
}