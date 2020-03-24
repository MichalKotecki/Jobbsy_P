using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Jobbsy.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

namespace Jobbsy.Controllers
{
    public class AdminController : Controller
    {

        private readonly ApplicationDbContext _context;
        private UserManager<IdentityUser> _userManager;
      

        public AdminController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
  
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> AddRolesToUsers()
        {
            List<IdentityUser> usersList =  _userManager.Users.ToList();
            List<String> rolesList =  new List<string>(new string[] {"Admin", "NormalUser", "Owner" });

            foreach (var item in usersList)
            {
                Debug.WriteLine(item);
            }
            foreach (var item in rolesList)
            {
                Debug.WriteLine(item);
            }

            Tuple<List<IdentityUser>, List<String>> collection = new Tuple<List<IdentityUser>, List<String>>(usersList, rolesList);


            return View(collection);
        }

        public async Task<IActionResult> AddRoleFinish()
        {
            string username = String.Format("{0}", Request.Form["username"]);
            string role = String.Format("{0}", Request.Form["role"]);

            IdentityUser user = await _userManager.FindByNameAsync(username);
            await _userManager.AddToRoleAsync(user, role);

            return View("Index");
        }

        
    }
}