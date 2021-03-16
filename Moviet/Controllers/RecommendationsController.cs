using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moviet.Data;
using Moviet.Models;
using Moviet.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Moviet.Controllers
{
    [Authorize(Roles = "ContentManager,Rater")]
    public class RecommendationsController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IRecommendationService _recommendationService;
        private readonly IMapper _mapper;

        public RecommendationsController(UserManager<ApplicationUser> userManager, IRecommendationService recommendationService, IMapper mapper)
        {
            _userManager = userManager;
            _recommendationService = recommendationService;
            _mapper = mapper;
        }


        public IActionResult Index()
        {
            var recommendations = _recommendationService.GetRecommendation(12, _userManager.GetUserId(User));
            var model = _mapper.Map<List<PostVM>>(recommendations);
            return View(model);
        }
    }
}
