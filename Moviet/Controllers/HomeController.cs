﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moviet.Contracts;
using Moviet.Data;
using Moviet.Models;
using Microsoft.AspNetCore.Http;

namespace Moviet.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IPostRepository _postrepo;
        private readonly IMapper _mapper;

        public HomeController(ILogger<HomeController> logger, IPostRepository postrepo, IMapper mapper)
        {
            _logger = logger;
            _postrepo = postrepo;
            _mapper = mapper;
        }

        public IActionResult Index()
        {
            List<Post> newReleases = _postrepo.FindAll();
            List<PostVM> model = _mapper.Map<List<PostVM>>(newReleases);
            ViewBag.NewReleases = model.OrderByDescending(p => p.DateCreated).Take(4).ToList();

            List<Post> topRated = _postrepo.FindAll();
            model = _mapper.Map<List<PostVM>>(topRated);
            ViewBag.TopRated = model.OrderByDescending(p => p.Movie.Rating).ToList();

            return View();
        }

        
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
