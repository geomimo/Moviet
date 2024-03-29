﻿using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
    [Authorize(Roles = "ContentManager")]
    public class PostsController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IPostRepository _postrepo;
        private readonly IGenreRepository _genrerepo;
        private readonly IRatingRepository _ratingrepo;
        private readonly IMovieRepository _movierepo;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IPosterUploadService _posterservice;
        private readonly IPornDetectorService _pornDetectorService;

        public PostsController(IPostRepository postrepo,
                               IMapper mapper,
                               IGenreRepository genrerepo,
                               IRatingRepository ratingrepo,
                               IMovieRepository movierepo,
                               UserManager<ApplicationUser> userManager,
                               IPosterUploadService posterservice,
                               IPornDetectorService pornDetectorService)
        {
            _mapper = mapper;
            _postrepo = postrepo;
            _genrerepo = genrerepo;
            _ratingrepo = ratingrepo;
            _movierepo = movierepo;
            _userManager = userManager;
            _posterservice = posterservice;
            _pornDetectorService = pornDetectorService;

        }

        public IActionResult Index(int? page, string searchString = null)
        {
            List<Post> posts = _postrepo.FindAllByUserId(_userManager.GetUserId(User));

            if (!String.IsNullOrEmpty(searchString))
            {
                ViewData["CurrentFilter"] = searchString;
                posts = posts.Where(p => p.Movie.Title.ToLower().Contains(searchString.ToLower())).ToList();
            }

            List<PostVM> model = _mapper.Map<List<PostVM>>(posts);

            int pageSize = 12;
            int pageNumber = (page ?? 1);

            return View(model.ToPagedList(pageNumber, pageSize));
        }

        public IActionResult Create()
        {
            var model = initPostModel();

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CreatePostVM model)
        {
            if (!ModelState.IsValid)
            {
                var return_model = initPostModel();

                return View(return_model);
            }

            Post post = _mapper.Map<Post>(model);
            post.Owner = (ApplicationUser)_userManager.GetUserAsync(User).Result;
            post.DateCreated = DateTime.Now;
            post.Movie.Ratings.First().DateRated = DateTime.Now;
            post.Movie.Ratings.First().Rater = (ApplicationUser)_userManager.GetUserAsync(User).Result;

            if (model.Movie.Poster != null)
            {
                post.Movie.PosterPath = _posterservice.UploadImage(model.Movie.Poster);
            }
            else
            {
                post.Movie.PosterPath = "82edbb4a-e688-4d7d-9ce8-06356a837ca9_noposter.jpg";
            }

            var textToCheck = post.Movie.Title + " " + post.Movie.LongDescription + " " + post.Movie.SortDescription;
            var isPorn = _pornDetectorService.IsPorn(post.Movie.PosterPath, textToCheck);

            if (isPorn)
            {
                var return_model = initPostModel();
                ViewData["PornMessage"] = "You cannot upload porn content!";
                return View(return_model);
            }

            post.IsNew = true;


            _postrepo.Create(post);

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Edit(int id)
        {
            Post post = _postrepo.FindById(id);
            if (post.Owner != _userManager.GetUserAsync(User).Result)
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
        [ValidateAntiForgeryToken]
        public IActionResult Edit(EditPostVM model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            Post newPost = _mapper.Map<Post>(model);
            Post oldPost = _postrepo.FindById(model.PostID);
            if (oldPost.Owner != _userManager.GetUserAsync(User).Result)
            {
                return RedirectToAction(nameof(Index));
            }

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

            if (model.Movie.Poster != null)
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
        [ValidateAntiForgeryToken]
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
    
        private CreatePostVM initPostModel()
        {
            var model = new CreatePostVM();
            model.Movie = new CreateMovieVM();
            model.Movie.AvailableGenres = new List<SelectListItem>();

            var availableGenres = _genrerepo.FindAll();
            foreach (var g in availableGenres)
            {
                model.Movie.AvailableGenres.Add(new SelectListItem { Text = g.Name, Value = g.GenreId.ToString() });
            }

            return model;
        }
    
    }
}
