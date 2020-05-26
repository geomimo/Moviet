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
        private readonly IMapper _mapper;

        public MoviesController(IPostRepository postrepo, IMapper mapper)
        {
            _postrepo = postrepo;
            _mapper = mapper;
        }   
        public IActionResult Index()
        {
            List<Post> posts = _postrepo.FindAll();
            List<PostVM> model = _mapper.Map<List<PostVM>>(posts);
            return View(model);
        }
    }
}