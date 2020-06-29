using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moviet.Contracts;
using Moviet.Data;
using Moviet.Models;
using System.Collections.Generic;
using System.Linq;

namespace Moviet.Controllers
{
    public class MoviesController : Controller
    {
        private readonly IPostRepository _postrepo;
        private readonly IGenreRepository _genrerepo;
        private readonly IMovieRepository _movierepo;
        private readonly IMapper _mapper;
        private readonly IRatingRepository _ratingrepo;
        private readonly UserManager<IdentityUser> _userManager;

        public MoviesController(IPostRepository postrepo,
                                IMapper mapper,
                                IGenreRepository genrerepo,
                                IMovieRepository movierepo,
                                IRatingRepository ratingrepo,
                                UserManager<IdentityUser> userManager)
        {
            _postrepo = postrepo;
            _genrerepo = genrerepo;
            _movierepo = movierepo;
            _mapper = mapper;
            _ratingrepo = ratingrepo;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            List<Post> posts = _postrepo.FindAll();
            List<PostVM> model = _mapper.Map<List<PostVM>>(posts);



            ViewData["Title"] = "All Movies";
            return View("Index", model);
        }

        public IActionResult TopRated()
        {
            List<Post> posts = _postrepo.FindAll();
            List<PostVM> model = _mapper.Map<List<PostVM>>(posts);
            model = model.OrderByDescending(p => p.Movie.Rating).ToList();

            ViewData["Title"] = "Top Rated";
            return View("Index", model);
        }

        public IActionResult NewReleases()
        {
            List<Post> posts = _postrepo.FindAll();
            List<PostVM> model = _mapper.Map<List<PostVM>>(posts);
            model = model.OrderByDescending(p => p.DateCreated).ToList();

            ViewData["Title"] = "New Releases";
            return View("Index", model);
        }

        public IActionResult ByGenre(int id)
        {
            List<Post> posts = _postrepo.FindAllByGenreId(id);
            List<PostVM> model = _mapper.Map<List<PostVM>>(posts);

            string genre = _genrerepo.FindById(id).Name;

            ViewData["Title"] = genre + " Movies";
            return View("Index", model);
        }


        public IActionResult Details(int id)
        {
            Post post = _postrepo.FindById(id);
            PostVM model = _mapper.Map<PostVM>(post);

            Rating userRating = _ratingrepo.FindAllByUserId(_userManager.GetUserId(User))
                                           .Where(r => r.Movie.MovieId == post.Movie.MovieId)
                                           .SingleOrDefault();

            model.Movie.UserRating = userRating;

            return View(model);
        }

        public IActionResult DetailsByMovie(int id)
        {
            Post post = _postrepo.FindAll().Where(p => p.Movie.MovieId == id).SingleOrDefault();

            return RedirectToAction(nameof(Details), new { id = post.PostId });
        }
    }
}