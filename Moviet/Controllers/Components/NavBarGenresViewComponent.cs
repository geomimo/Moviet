using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using Moviet.Contracts;
using Moviet.Data;
using Moviet.Models;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Moviet.Controllers.Components
{
    public class NavBarGenresViewComponent : ViewComponent
    {
        private readonly IGenreRepository _genrerepo;
        private readonly IMapper _mapper;

        public NavBarGenresViewComponent(IGenreRepository genrerepo, IMapper mapper)
        {
            _genrerepo = genrerepo;
            _mapper = mapper;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            List<Genre> genres = await _genrerepo.FindAllAsync();
            List<GenreVM> model = _mapper.Map<List<GenreVM>>(genres);
            return View(model);

        }
    }
}
