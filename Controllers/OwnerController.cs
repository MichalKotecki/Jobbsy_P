using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Jobbsy.Controllers
{
    public class OwnerController : Controller
    {
        [Authorize(Roles = "Owner")]
        public IActionResult Index()
        {
            return View();
        }
    }
}