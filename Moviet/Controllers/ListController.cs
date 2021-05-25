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
using X.PagedList;

namespace Moviet.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class ListController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;
        private readonly IBanService _banService;
        private readonly IPostRepository _postrepo;


        public ListController(UserManager<ApplicationUser> userManager,
                              IMapper mapper,
                              IBanService banService,
                              IPostRepository postrepo)
        {
            _userManager = userManager;
            _mapper = mapper;
            _banService = banService;
            _postrepo = postrepo;
        }

        public IActionResult AllUsers(int? page, string searchString = null)
        {
            var raters = _userManager.GetUsersInRoleAsync(Roles.Rater).Result;
            var contentManager = _userManager.GetUsersInRoleAsync(Roles.ContentManager).Result;
            var allUsers = new List<ApplicationUser>();
            allUsers.AddRange(raters);
            allUsers.AddRange(contentManager);
            allUsers.Sort((p, q) => p.UserName.CompareTo(q.UserName));

            if (!String.IsNullOrEmpty(searchString))
            {

                ViewData["CurrentFilter"] = searchString;
                allUsers = allUsers.Where(s => s.UserName.ToLower().Contains(searchString.ToLower())
                                       || s.Email.ToLower().Contains(searchString.ToLower())).ToList();
            }

            var model = _mapper.Map<List<IdentityUserVM>>(allUsers);

            foreach (var u in allUsers)
            {
                var role = _userManager.GetRolesAsync(u).Result.First();
                var m = model.Find(uvm => uvm.Id == u.Id);
                m.Role = role;
            }

            int pageSize = 12;
            int pageNumber = (page ?? 1);

            return View(model.ToPagedList(pageNumber, pageSize));
        }

        public IActionResult AllPosts(int? page, string searchString = null)
        { 
            var posts = _postrepo.FindAll();
            posts.Sort((p, q) => p.DateCreated.CompareTo(q.DateCreated));

            if (!String.IsNullOrEmpty(searchString))
            {        
                ViewData["CurrentFilter"] = searchString;  
                posts = posts.Where(s => s.Movie.Title.ToLower().Contains(searchString.ToLower())).ToList();
            }      

            var model = _mapper.Map<List<PostVM>>(posts);

            int pageSize = 12;
            int pageNumber = (page ?? 1);
            return View(model.ToPagedList(pageNumber, pageSize));
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