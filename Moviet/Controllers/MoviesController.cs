using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Moviet.Contracts;

namespace Moviet.Controllers
{
    public class MoviesController : Controller
    {
        private readonly IMapper _mapper;
        
        public MoviesController(IMovieRepository movierepo, IMapper mapper)
        {
            _mapper = mapper;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Details()
        {
            return View();
        }
    }
}
