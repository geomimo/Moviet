using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
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
        private readonly IPostRepository _postrepo;
        private readonly IGenreRepository _genrerepo;
        private readonly IMapper _mapper;

        public MoviesController(IPostRepository postrepo, IMapper mapper, IGenreRepository genrerepo)
        {
            _postrepo = postrepo;
            _genrerepo = genrerepo;
            _mapper = mapper;
        }

        public IActionResult Index()
        {
            List<Post> posts = _postrepo.FindAll();
            List<PostVM> model = _mapper.Map<List<PostVM>>(posts);

            List<Genre> genres = _genrerepo.FindAll();
            List<GenreVM> genresVM = _mapper.Map<List<GenreVM>>(genres); 


            ViewData["Title"] = "All Movies";
            return View("Index", model);
        }

        public IActionResult TopRated()
        {
            List<Post> posts = _postrepo.FindAll();
            List<PostVM> model = _mapper.Map<List<PostVM>>(posts);         
            model = model.OrderByDescending(p => p.Movie.Rating).ToList();

            ViewData["Title"] = "Top Rated";
            return View("Index", model);
        }

        public IActionResult NewReleases()
        {
            List<Post> posts = _postrepo.FindAll();
            List<PostVM> model = _mapper.Map<List<PostVM>>(posts); 
            model = model.OrderByDescending(p => p.DateCreated).ToList();

            ViewData["Title"] = "New Releases";
            return View("Index", model);
        }

        public IActionResult ByGenre(int id)
        {
            List<Post> posts = _postrepo.FindAllByGenreId(id);
            List<PostVM> model = _mapper.Map<List<PostVM>>(posts);

            string genre = _genrerepo.FindById(id).Name;

            ViewData["Title"] = genre + " Movies";
            return View("Index", model);
        }


        public IActionResult Details(int postId)
        {
            Post post = _postrepo.FindById(postId);
            PostVM model = _mapper.Map<PostVM>(post);
            return View(model);
        }
    }
}