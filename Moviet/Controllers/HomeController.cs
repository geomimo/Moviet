using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Moviet.Contracts;
using Moviet.Data;
using Moviet.Models;
using System.Collections.Generic;
using System.Linq;
using MovietML.Model;
using System.Web;
using Moviet.Services.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace Moviet.Controllers
{
    public class HomeController : Controller
    {
        private readonly IPostRepository _postrepo;
        private readonly IMapper _mapper;
        private readonly IRecommendationService _recommendationService;
        private readonly UserManager<ApplicationUser> _userManager;

        public HomeController(IPostRepository postrepo,
                              IMapper mapper,
                              IRecommendationService recommendationService,
                              UserManager<ApplicationUser> userManager)
        {
            _postrepo = postrepo;
            _mapper = mapper;
            _recommendationService = recommendationService;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            List<Post> newReleases = _postrepo.FindAll();
            List<PostVM> model = _mapper.Map<List<PostVM>>(newReleases);
            ViewBag.NewReleases = model.OrderByDescending(p => p.DateCreated).Take(4).ToList();

            List<Post> topRated = _postrepo.FindAll();
            model = _mapper.Map<List<PostVM>>(topRated);
            ViewBag.TopRated = model.OrderByDescending(p => p.Movie.Rating).Take(4).ToList();

            bool isLogged = User?.Identity.IsAuthenticated == true;
            model = null;
            if (isLogged)
            {
                var recommendations = _recommendationService.GetRecommendation(4, _userManager.GetUserId(User));
                model = _mapper.Map<List<PostVM>>(recommendations);
            }
            ViewBag.Recommendations = model;

            return View();
        }


    }
}
