using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
        private readonly IGenreRepository _genrerepo;

        public PostsController(IPostRepository postrepo, IMapper mapper, IGenreRepository genrerepo)
        {
            _mapper = mapper;
            _postrepo = postrepo;
            _genrerepo = genrerepo;
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

        public IActionResult Create()
        {
            var model = new CreatePostVM();
            model.Movie = new CreateMovieVM();
            model.Movie.Genres = new List<SelectListItem>();

            var availableGenres = _genrerepo.FindAll();
            foreach(var g in availableGenres)
            {
                model.Movie.Genres.Add(new SelectListItem { Text = g.Name, Value = g.Name });
            }

            return View(model);
        }

        [HttpPost]
        public IActionResult Create(PostVM post)
        {
            
        }
    }
}
