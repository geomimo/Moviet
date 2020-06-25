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
        private readonly IMovieRepository _movierepo;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IPosterUploadService _posterservice;

        public PostsController(IPostRepository postrepo,
                               IMapper mapper,
                               IGenreRepository genrerepo,
                               IRatingRepository ratingrepo,
                               IMovieRepository movierepo,
                               UserManager<IdentityUser> userManager,
                               IPosterUploadService posterservice)
        {
            _mapper = mapper;
            _postrepo = postrepo;
            _genrerepo = genrerepo;
            _ratingrepo = ratingrepo;
            _movierepo = movierepo;
            _userManager = userManager;
            _posterservice = posterservice;
            
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

            Post post = _mapper.Map<Post>(model);
            post.Owner = _userManager.GetUserAsync(User).Result;
            post.DateCreated = DateTime.Now;
            post.Movie.Ratings.First().DateRated = DateTime.Now;
            post.Movie.Ratings.First().Rater = _userManager.GetUserAsync(User).Result;

            if (model.Movie.Poster != null)
            {
                post.Movie.PosterPath = _posterservice.UploadImage(model.Movie.Poster);
            }
            else
            {
                post.Movie.PosterPath = "82edbb4a-e688-4d7d-9ce8-06356a837ca9_noposter.jpg";
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
            model.Movie.Rating = _ratingrepo.FindAllByUserId(_userManager.GetUserId(User))
                                                  .Single(r => r.Movie.MovieId == model.Movie.MovieId);

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
            Post oldPost = _postrepo.FindById(model.PostID);

            if (newPost.Movie.Title != null)
            {
                oldPost.Movie.Title = newPost.Movie.Title;
            }

            if (newPost.Movie.SortDescription != null)
            {
                oldPost.Movie.SortDescription = newPost.Movie.SortDescription;
            }

            if (newPost.Movie.LongDescription != null)
            {
                oldPost.Movie.LongDescription = newPost.Movie.LongDescription;
            }

            var newRating = newPost.Movie.Ratings.First();
            var oldRating = oldPost.Movie.Ratings.Single(r => r.RaterId == oldPost.OwnerId);
            if (newRating.Value != oldRating.Value)
            {
                oldRating.Value = newRating.Value;
                oldRating.DateRated = DateTime.Now;
            }

            if(model.Movie.Poster != null)
            {
                oldPost.Movie.PosterPath = _posterservice.UploadImage(model.Movie.Poster);
            }

            var newUrl = newPost.Movie.YoutubeId;
            var oldUrl = oldPost.Movie.YoutubeId;
            if (newUrl != oldUrl)
            {
                oldPost.Movie.YoutubeId = newUrl;
            }

            oldPost.Movie.Genres = newPost.Movie.Genres;
            _postrepo.Update(oldPost);


            return RedirectToAction(nameof(Index));
        }
        
        public IActionResult Delete(int id)
        {
            Post post = _postrepo.FindById(id);
            if (post.Owner != _userManager.GetUserAsync(User).Result)
            {
                return RedirectToAction(nameof(Index));
            }

            PostVM model = _mapper.Map<PostVM>(post);
            return View(model);
        }

        [HttpPost]
        public IActionResult Delete(IFormCollection form)
        {

            int id = Int32.Parse(form["PostID"]);
            Post post = _postrepo.FindById(id);
            if (post.Owner != _userManager.GetUserAsync(User).Result)
            {
                return RedirectToAction(nameof(Index));
            }
            Movie movie = _movierepo.FindById(post.Movie.MovieId);
            movie.PostRemoved = true;
            _movierepo.Update(movie);
            _postrepo.Delete(post);
            return RedirectToAction(nameof(Index));
        }
    }
}
