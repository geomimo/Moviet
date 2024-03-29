﻿using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Moviet.Contracts;
using Moviet.Data;
using Moviet.Models;
using System.Collections.Generic;
using System.Linq;

namespace Moviet.Controllers
{
    public class HomeController : Controller
    {
        private readonly IPostRepository _postrepo;
        private readonly IMapper _mapper;

        public HomeController(IPostRepository postrepo, IMapper mapper)
        {
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
            ViewBag.TopRated = model.OrderByDescending(p => p.Movie.Rating).Take(4).ToList();

            return View();
        }


    }
}
