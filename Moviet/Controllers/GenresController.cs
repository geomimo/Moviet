using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moviet.Contracts;
using Moviet.Data;
using Moviet.Models;

namespace Moviet.Controllers
{
    [Authorize(Roles="Administrator")]
    public class GenresController : Controller
    {
        private readonly IGenreRepository _genrerepo;
        private readonly IMapper _mapper;

        public GenresController(IGenreRepository genrerepo, IMapper mapper)
        {
            _genrerepo = genrerepo;
            _mapper = mapper;
        }

        public IActionResult Index()
        {
            List<Genre> genres = _genrerepo.FindAll();
            List<GenreVM> model = _mapper.Map<List<GenreVM>>(genres);
            
            return View(model);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(GenreVM model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            Genre genre = _mapper.Map<Genre>(model);
            _genrerepo.Create(genre);
            
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Edit(int id)
        {
            Genre genre = _genrerepo.FindById(id);
            GenreVM model = _mapper.Map<GenreVM>(genre);
            return View(model);
        }

        [HttpPost]
        public IActionResult Edit(GenreVM model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            Genre genre = _mapper.Map<Genre>(model);
            _genrerepo.Update(genre);

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int id)
        {
            Genre genre = _genrerepo.FindById(id);
            _genrerepo.Delete(genre);

            return RedirectToAction(nameof(Index));
        }


    }
}