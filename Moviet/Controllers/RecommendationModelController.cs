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
using System.Threading.Tasks;

namespace Moviet.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class RecommendationModelController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IPostRepository _postrepo;
        private readonly IRecommendationService _recommendationService;
        private readonly IEvaluationResultsRepository _evaluationResultsRepo;
        private readonly IMapper _mapper;

        public RecommendationModelController(UserManager<ApplicationUser> userManager,
                                             IPostRepository postrepo,
                                             IRecommendationService recommendationService,
                                             IEvaluationResultsRepository evaluationResultsRepo,
                                             IMapper mapper)
        {
            _userManager = userManager;
            _postrepo = postrepo;
            _recommendationService = recommendationService;
            _evaluationResultsRepo = evaluationResultsRepo;
            _mapper = mapper;
        }

        public IActionResult Index()
        {
            var users = _userManager.Users;
            var posts = _postrepo.FindAll();

            ViewData["numberOfUsers"] = users.Count();
            ViewData["numberOfNewUsers"] = users.Where(u => u.IsNew).Count();

            ViewData["numberOfPosts"] = posts.Count();
            ViewData["numberOfNewPosts"] = posts.Where(p => p.IsNew).Count();


            var results = _evaluationResultsRepo.FindAll();
            var model = _mapper.Map<List<EvaluationResultsVM>>(results);
            return View(model);
        }

        public IActionResult Train()
        {
            _recommendationService.Train();
            return RedirectToAction(nameof(Index));
        }
    }
}
