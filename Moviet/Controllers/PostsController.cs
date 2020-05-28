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
        private readonly IRatingRepository _ratingrepo;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IPosterUploadService _posterservice;
        private readonly IYoutubeService _ytservice;

        public PostsController(IPostRepository postrepo,
                               IMapper mapper,
                               IGenreRepository genrerepo,
                               IRatingRepository ratingrepo,
                               UserManager<IdentityUser> userManager,
                               IPosterUploadService posterservice,
                               IYoutubeService ytservice)
        {
            _mapper = mapper;
            _postrepo = postrepo;
            _genrerepo = genrerepo;
            _ratingrepo = ratingrepo;
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
            post.Movie.Ratings.First().DateRated = DateTime.Now;
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

        public IActionResult Edit(int id)
        {
            Post post = _postrepo.FindById(id);
            if(post.Owner != _userManager.GetUserAsync(User).Result)
            {
                return RedirectToAction(nameof(Index));
            }

            EditPostVM model = _mapper.Map<EditPostVM>(post);
            model.Movie.AvailableGenres = new List<SelectListItem>();

            // Add available genres
            var availableGenres = _genrerepo.FindAll();
            foreach (var g in availableGenres)
            {
                model.Movie.AvailableGenres.Add(new SelectListItem 
                                                { 
                                                    Text = g.Name, 
                                                    Value = g.GenreId.ToString(), 
                                                    Selected = post.Movie.Genres.Exists(q => q.GenreId == g.GenreId) 
                                                });
            }

            // Add owner's rating.
            model.Movie.Rating = _ratingrepo.FindAllByUsersId(_userManager.GetUserId(User))
                                                  .Single(r => r.MovieId == model.Movie.MovieId);

            return View(model);
        }

        [HttpPost]
        public IActionResult Edit(EditPostVM model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            Post newPost = _mapper.Map<Post>(model);
            newPost.DateCreated = DateTime.Now;

            newPost.Movie.Ratings.First().DateRated = DateTime.Now;
            //newPost.Movie.Ratings.First().RaterId = _userManager.GetUserId(User);
            //newPost.Movie.Ratings.First().MovieId = newPost.Movie.MovieId;
            if (model.Movie.Poster != null)
            {
                newPost.Movie.PosterPath = _posterservice.UploadImage(model.Movie.Poster);
            }

            if (model.Movie.YoutubeId != null)
            {
                newPost.Movie.YoutubeId = _ytservice.ConvertUrl(model.Movie.YoutubeId);
            }

            var r = _postrepo.Update(newPost);


            return RedirectToAction(nameof(Index));
        }
    }
}
