using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moviet.Contracts;
using Moviet.Data;
using Moviet.Models;
using Moviet.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
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
        private readonly IPostRepository _postrepo;
        private readonly IRatingRepository _ratingrepo;
        private static List<Post> _showedMovies;
        

        public AccountController(UserManager<ApplicationUser> userManager,
                                 SignInManager<ApplicationUser> signInManager,
                                 IRoleService roleService,
                                 IMapper mapper,
                                 IPostRepository postrepo,
                                 IRatingRepository ratingrepo)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleService = roleService;
            _mapper = mapper;
            _postrepo = postrepo;
            _ratingrepo = ratingrepo;

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


        public IActionResult SetPreferences(int id = -1)
        {
            if(id != -1)
            {
                var post = _postrepo.FindById(id);
                var rating = new Rating()
                {
                    MovieId = post.Movie.MovieId,
                    RaterId = _userManager.GetUserId(User),
                    Value = 5.0F,
                    DateRated = DateTime.Now
                };

                _ratingrepo.Create(rating);
            }
            else
            {
                _showedMovies = new List<Post>();
            }

            if(_showedMovies.Count == 4 * 5)
            {
                _showedMovies.Clear();
                return RedirectToAction("Index", "Home");
            }

            var posts = _postrepo.FindAll();
            posts = posts.OrderByDescending(p => p.Movie.Ratings.Average(r => r.Value)).ToList();
            var prefToShow = new  List<Post>();
            int threshold = 4;
            for(int i = 0; i < posts.Count; i++)
            {
                if (prefToShow.Count == threshold) break;

                if (_showedMovies.Select(p=>p.PostId).Any(pid => pid == posts[i].PostId)) continue;

                prefToShow.Add(posts[i]);
                _showedMovies.Add(posts[i]);
            }

            var model = _mapper.Map<List<PostVM>>(prefToShow);
            return View(model);
        }
    }
}