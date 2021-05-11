using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moviet.Contracts;
using Moviet.Data;
using Moviet.Models;
using System.Collections.Generic;
using System.Linq;
using X.PagedList;

namespace Moviet.Controllers
{
    public class MoviesController : Controller
    {
        private readonly IPostRepository _postrepo;
        private readonly IGenreRepository _genrerepo;
        private readonly IMovieRepository _movierepo;
        private readonly IMapper _mapper;
        private readonly IRatingRepository _ratingrepo;
        private readonly UserManager<ApplicationUser> _userManager;

        public MoviesController(IPostRepository postrepo,
                                IMapper mapper,
                                IGenreRepository genrerepo,
                                IMovieRepository movierepo,
                                IRatingRepository ratingrepo,
                                UserManager<ApplicationUser> userManager)
        {
            _postrepo = postrepo;
            _genrerepo = genrerepo;
            _movierepo = movierepo;
            _mapper = mapper;
            _ratingrepo = ratingrepo;
            _userManager = userManager;
        }

        public IActionResult Index(int? page, string searchString = null, int genreId = -1, bool topRated = false, bool newReleases = false)
        {
            List<Post> posts = new List<Post>();
            if (!string.IsNullOrEmpty(searchString))
            {
                posts = _postrepo.FindAll();
                posts = posts.Where(p => p.Movie.Title.ToLower().Contains(searchString.ToLower())).ToList();
                ViewData["Title"] = "Seached for " + searchString;
            }
            else if (genreId != -1)
            {
                posts = _postrepo.FindAllByGenreId(genreId);
                Genre genre = _genrerepo.FindById(genreId);
                string name = "";
                if (genre != null)
                {
                    name = genre.Name;
                }

                ViewData["Title"] = name + " Movies";
            }
            else if (topRated)
            {
                posts = _postrepo.FindAll();
                posts = posts.OrderByDescending(p => p.Movie.Ratings.Sum(r => r.Value)).ToList();
                ViewData["Title"] = "Top Rated";
            }
            else if (newReleases)
            {
                posts = _postrepo.FindAll();
                posts = posts.OrderByDescending(p => p.DateCreated).ToList();
            }
            else
            {
                posts = _postrepo.FindAll();
            }

            ViewBag.searchString = searchString;
            ViewBag.genreId = genreId;
            ViewBag.topRated = topRated;
            ViewBag.newReleases = newReleases;



            List<PostVM> model = _mapper.Map<List<PostVM>>(posts);

            int pageSize = 12;
            int pageNumber = (page ?? 1);

            return View("Index", model.ToPagedList(pageNumber, pageSize));
        }


        public IActionResult Details(int id)
        {
            Post post = _postrepo.FindById(id);
            if(post == null)
            {
                return NotFound();
            }
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