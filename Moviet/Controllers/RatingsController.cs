using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moviet.Contracts;
using Moviet.Data;
using Moviet.Models;

namespace Moviet.Controllers
{
    [Authorize(Roles = "ContentManager,Rater")]
    public class RatingsController : Controller
    {
        private readonly IRatingRepository _ratingrepo;
        private readonly IPostRepository _postrepo;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IMapper _mapper;

        public RatingsController(IRatingRepository ratingrepo,
                                 IPostRepository postrepo,
                                 UserManager<IdentityUser> userManager,
                                 IMapper mapper)
        {
            _ratingrepo = ratingrepo;
            _postrepo = postrepo;
            _userManager = userManager;
            _mapper = mapper;
        }

        public IActionResult Index()
        {
            List<Rating> ratings = _ratingrepo.FindAllByUserId(_userManager.GetUserId(User));
            List<RatingVM> model = _mapper.Map<List<RatingVM>>(ratings);
            return View(model);
        }

        public IActionResult Rate(IFormCollection form)
        {
            Post post = _postrepo.FindById(Int32.Parse(form["PostId"]));
            List<Rating> ratings = _ratingrepo.FindAllByUserId(_userManager.GetUserId(User));

            // Check if user has already rated the movie. 
            Rating rating = ratings.Where(r => r.Movie.MovieId == post.Movie.MovieId).FirstOrDefault();
            if(rating != null)
            {
                rating.Value = float.Parse(form["Rating"]);
                rating.DateRated = DateTime.Now;
                _ratingrepo.Update(rating);
            }
            else
            {
                rating = new Rating
                {
                    DateRated = DateTime.Now,
                    Movie = post.Movie,
                    Rater = _userManager.GetUserAsync(User).Result,
                    Value = float.Parse(form["Rating"])
                };
                _ratingrepo.Create(rating);
            }

            return RedirectToAction("Details", "Movies", new { id = post.PostId });
        }
    
        public IActionResult Edit(int id)
        {
            Rating rating = _ratingrepo.FindById(id);
            RatingVM model = _mapper.Map<RatingVM>(rating);

            return View(model);
        }

        [HttpPost]
        public IActionResult Edit(RatingVM model)
        {
            Rating rating = _ratingrepo.FindById(model.RatingId);
            rating.Value = model.Value;
            rating.DateRated = DateTime.Now;

            _ratingrepo.Update(rating);

            return RedirectToAction(nameof(Index));

        }
    }
}