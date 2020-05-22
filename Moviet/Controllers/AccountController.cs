using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moviet.Models;

namespace Moviet.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Index()
        {
            return View(new IdentityUserVM { Username = "Geomimo", Email = "mimoglou22@gmail.com" });
        }
    }
}