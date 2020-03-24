using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Jobbsy.Data;
using Jobbsy.Models;
using X.PagedList;

namespace Jobbsy.Controllers
{
    public class CompaniesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CompaniesController(ApplicationDbContext context)
        {
            _context = context;
        }

        //GET: Companies
        public async Task<IActionResult> Index()
        {
            return View(await _context.Company.ToListAsync());
        }


        public async Task<IActionResult> CompanyListForUser(int? page)
        {
            int pageSize = 15;
            int pageNumber = (page ?? 1);
            var companyList = await _context.Company.ToListAsync();
            
            return View(companyList.ToPagedList(pageNumber, pageSize));
        }

        public async Task<IActionResult> CompanyListForSearchResult()
        {

            string tech1 = String.Format("{0}", Request.Form["tech1"]);
            string tech2 = String.Format("{0}", Request.Form["tech2"]);
            string tech3 = String.Format("{0}", Request.Form["tech3"]);
            string mode = String.Format("{0}", Request.Form["mode"]);

            // TODO

            var companyList = await _context.Company
                .Include(p => p.PhotoCollection)
                .Include(tc => tc.TechnologyCompany)
                .ThenInclude(tc => tc.Technology)
                .Include(com => com.CommentCollection)
                .ToListAsync<Company>();

            List<Company> filteredCompanyList = new List<Company>();

            if(mode == "0") // All
            {
                foreach (var company in companyList)
                {
                    if (company.TechnologyCompany.Any(t => t.Technology.technologyID.ToString() == tech1) && company.TechnologyCompany.Any(t => t.Technology.technologyID.ToString() == tech2) && company.TechnologyCompany.Any(t => t.Technology.technologyID.ToString() == tech3))
                    {
                        filteredCompanyList.Add(company);
                    }
                }
            }
            else // Any
            {
                foreach (var company in companyList)
                {
                    if (company.TechnologyCompany.Any(t => t.Technology.technologyID.ToString() == tech1 || t.Technology.technologyID.ToString() == tech2 || t.Technology.technologyID.ToString() == tech3))
                    {
                        filteredCompanyList.Add(company);
                    }
                }
            }


            

           
            if (filteredCompanyList == null)
            {
                return NotFound();
            }

            return View(filteredCompanyList);
        }



        public async Task<IActionResult> CompanySearchWindow()
        {
            return View(5);
        }


        public async Task<IActionResult> CategorySearchResult(int? page)
        {
            var companyList = await _context.Company.ToListAsync();

            double centerX = 19.456916;
            double centerY = 51.767470;

            List<Tuple<string, string, string>> company_AND_placement = new List<Tuple<string, string, string>>();


            foreach (Company com in companyList)
            {
                string comX = "";
                string comY = "";

                if (com.locationX < centerX)
                {
                    comX = "WEST";
                }
                else
                {
                    comX = "EAST";
                }


                if (com.locationY < centerY)
                {
                    comY = "SOUTH";
                }
                else
                {
                    comY = "NORTH";
                }


                company_AND_placement.Add(new Tuple<string, string, string>(com.companyName, comX, comY));

            }

            var company = await _context.Company
               .Include(p => p.PhotoCollection)
               .Include(tc => tc.TechnologyCompany)
               .ThenInclude(tc => tc.Technology)
               .Include(com => com.CommentCollection)
               .ToListAsync();

            List<Company> companyFilteredList = new List<Company>();

            string choosenX = String.Format("{0}", Request.Form["LocationX"]);
            string choosenY = String.Format("{0}", Request.Form["LocationY"]);

            foreach (var item in company_AND_placement)
            {
                Debug.Write(choosenX); Debug.WriteLine(item.Item2);
                Debug.Write(choosenY); Debug.WriteLine(item.Item3);
                Debug.WriteLine(" ");

                if (item.Item2 == choosenX && item.Item3 == choosenY)
                {
                    companyFilteredList.Add(company.Where(c => c.companyName == item.Item1).FirstOrDefault());
                }
            }

            int pageSize = 15;
            int pageNumber = (page ?? 1);

            return View(companyFilteredList.ToPagedList(pageNumber, pageSize));
        }





        // GET: Companies/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            TempData["companyID"] = id;
            

            var company = await _context.Company
                .Include(p => p.PhotoCollection)
                .Include(tc => tc.TechnologyCompany)
                .ThenInclude(tc => tc.Technology)
                .Include(com => com.CommentCollection)
                .FirstOrDefaultAsync(m => m.companyID == id);
            if (company == null)
            {
                return NotFound();
            }

            return View(company);
        }

        // GET: Companies/Create
        public IActionResult Create()
        {

            return View();
        }


        // POST: Companies/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("companyID,companyName,description,locationX,locationY,website")] Company company)
        {
            if (ModelState.IsValid)
            {
                _context.Add(company);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(company);
        }

        // GET: Companies/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var company = await _context.Company.FindAsync(id);
            if (company == null)
            {
                return NotFound();
            }
            return View(company);
        }

        // POST: Companies/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("companyID,companyName,description,locationX,locationY,website")] Company company)
        {
            if (id != company.companyID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(company);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CompanyExists(company.companyID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(company);
        }

        // GET: Companies/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var company = await _context.Company
                .FirstOrDefaultAsync(m => m.companyID == id);
            if (company == null)
            {
                return NotFound();
            }

            return View(company);
        }

        // POST: Companies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var company = await _context.Company.FindAsync(id);
            _context.Company.Remove(company);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CompanyExists(int id)
        {
            return _context.Company.Any(e => e.companyID == id);
        }
    }
}
