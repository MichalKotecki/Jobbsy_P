using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Jobbsy.Models;
using Jobbsy.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Jobbsy.Controllers
{
    public class HomeController : Controller
    {

        private readonly ApplicationDbContext _context;
        private UserManager<IdentityUser> _userManager;

        public HomeController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Perform()
        {
           
            //Company company = await _context.Company.Where(p => p.companyName == "Comarch").Include(p => p.PhotoCollection).FirstOrDefaultAsync();
            //List<Photo> photos = company.PhotoCollection.ToList();
            //TempData["test"] = System.Text.Json.JsonSerializer.Serialize(photos);

            return RedirectToAction("Index");
        }


        public async Task<IActionResult> Index()
        {

            if (User.Identity.Name != null && _userManager != null)
            {
                IdentityUser user = await _userManager.FindByNameAsync(User.Identity.Name);
                await _userManager.AddToRoleAsync(user, "NormalUser");

                var currentUserRoles = await _userManager.GetRolesAsync(user);
                foreach (var role in currentUserRoles)
                {
                    Debug.WriteLine(role);
                }

            }


            List<Company> companyList = await _context.Company.Select(s => s).ToListAsync<Company>();
           // Debug.Write("Lista: "); Debug.WriteLine(System.Text.Json.JsonSerializer.Serialize(companyList));
            TempData["companyList"] = System.Text.Json.JsonSerializer.Serialize(companyList);
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}


