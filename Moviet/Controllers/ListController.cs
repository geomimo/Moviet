using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moviet.Contracts;
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
        private readonly IPostRepository _postrepo;

        public ListController(UserManager<IdentityUser> userManager,
                              IMapper mapper,
                              IBanService banService,
                              IPostRepository postrepo)
        {
            _userManager = userManager;
            _mapper = mapper;
            _banService = banService;
            _postrepo = postrepo;
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

            foreach (var u in allUsers)
            {
                var role = _userManager.GetRolesAsync(u).Result.First();
                var m = model.Find(uvm => uvm.Id == u.Id);
                m.Role = role;                
            }

            return View(model);
        }   

        public IActionResult AllPosts()
        {
            var posts = _postrepo.FindAll();
            posts.Sort((p, q) => p.DateCreated.CompareTo(q.DateCreated));
            var model = _mapper.Map<List<PostVM>>(posts);

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

        public IActionResult BanPost(int id)
        {
            var banned = _banService.BanPost(id);
            if (!banned)
            {
                return NotFound();
            }

            return RedirectToAction(nameof(AllPosts));

        }
    }
}