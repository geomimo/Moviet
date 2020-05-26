using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Moviet.Contracts;
using Moviet.Data;
using Moviet.Models;
using Moviet.Services.Interfaces;

namespace Moviet.Controllers
{
    [Authorize(Roles = "ContentManager")]
    public class PostsController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IPostRepository _postrepo;
        private readonly IGenreRepository _genrerepo;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IPosterUploadService _posterservice;
        private readonly IYoutubeService _ytservice;

        public PostsController(IPostRepository postrepo,
                               IMapper mapper,
                               IGenreRepository genrerepo,
                               UserManager<IdentityUser> userManager,
                               IPosterUploadService posterservice,
                               IYoutubeService ytservice)
        {
            _mapper = mapper;
            _postrepo = postrepo;
            _genrerepo = genrerepo;
            _userManager = userManager;
            _posterservice = posterservice;
            _ytservice = ytservice;
        }

        public IActionResult Index()
        {
            List<Post> posts = _postrepo.FindAllByUserId(_userManager.GetUserId(User));
            List<PostVM> model = _mapper.Map<List<PostVM>>(posts);
            return View(model);
        }

        public IActionResult Details(int id)
        {
            //var Movie = _movierepo.FindById(id);
            //var model = _mapper.Map<MovieVM>(Movie);

            var model = new MovieVM
            {
                Description = "this is a description",
                MovieId = 1,
                Title = "Avengers"
            };
            return View(model);
        }

        public IActionResult Create()
        {
            var model = new CreatePostVM();
            model.Movie = new CreateMovieVM();
            model.Movie.AvailableGenres = new List<SelectListItem>();

            var availableGenres = _genrerepo.FindAll();
            foreach(var g in availableGenres)
            {
                model.Movie.AvailableGenres.Add(new SelectListItem { Text = g.Name, Value = g.GenreId.ToString()});
            }

            return View(model);
        }

        [HttpPost]
        public IActionResult Create(CreatePostVM model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            string userId = _userManager.GetUserId(User);

            Post post = _mapper.Map<Post>(model);
            post.Owner = _userManager.GetUserAsync(User).Result;
            post.DateCreated = DateTime.Now;
            post.Movie.Ratings.First().Rater = _userManager.GetUserAsync(User).Result;

            if (model.Movie.Poster != null)
            {
                post.Movie.PosterPath = _posterservice.UploadImage(model.Movie.Poster);
            }

            if(model.Movie.YoutubeId != null)
            {
                post.Movie.YoutubeId = _ytservice.ConvertUrl(model.Movie.YoutubeId);
            }


            _postrepo.Create(post);

            return RedirectToAction(nameof(Index));
        }
    }
}
