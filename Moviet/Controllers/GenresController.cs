using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Moviet.Controllers
{
    public class GenresController : Controller
    {
        public GenresController()
        {

        }

        public IActionResult Index()
        {
            return View();
        }
    }
}