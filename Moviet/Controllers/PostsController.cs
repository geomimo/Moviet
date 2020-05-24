using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Moviet.Contracts;
using Moviet.Data;
using Moviet.Models;

namespace Moviet.Controllers
{
    public class PostsController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IPostRepository _postrepo;

        public PostsController(IPostRepository postrepo, IMapper mapper)
        {
            _mapper = mapper;
            _postrepo = postrepo;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Details(int id)
        {
            //var Movie = _movierepo.FindById(id);
            //var model = _mapper.Map<MovieVM>(Movie);

            var model = new MovieVM
            {
                Description = "this is a description",
                MovieId = 1,
                Title = "Avengers"
        };
            return View(model);
        }
    }
}
