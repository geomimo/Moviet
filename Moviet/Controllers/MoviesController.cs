using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Moviet.Contracts;
using Moviet.Data;
using Moviet.Models;

namespace Moviet.Controllers
{
    public class MoviesController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IMovieRepository _movierepo;

        public MoviesController(IMovieRepository movierepo, IMapper mapper)
        {
            _mapper = mapper;
            _movierepo = movierepo;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Details(int id)
        {
            var movie = _movierepo.Find(id);
            var model = _mapper.Map<MovieVM>(movie);
            return View(model);
        }
    }
}
