using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Moviet.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class BanController : Controller
    {
        public BanController(IBanService banService)
        {
            _banService = banService;
        }

        public IActionResult BanUser(string userId)
        {
            
        } 
    }
}