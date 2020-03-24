using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Jobbsy.Data;
using Jobbsy.Models;

namespace Jobbsy.Controllers
{
    public class TechnologiesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TechnologiesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Technologies
        public async Task<IActionResult> Index()
        {
            return View(await _context.Technology.ToListAsync());
        }


        public async Task<IActionResult> Search()
        {
            List<SelectListItem> technologyList = await _context.Technology.Select(tech => new SelectListItem { Value = tech.technologyID.ToString(), Text = tech.name }).ToListAsync<SelectListItem>();
    
            return View(await _context.Technology.ToListAsync());
        }

        // GET: Technologies/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var technology = await _context.Technology
                .FirstOrDefaultAsync(m => m.technologyID == id);
            if (technology == null)
            {
                return NotFound();
            }

            return View(technology);
        }

        // GET: Technologies/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Technologies/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("technologyID,name,description")] Technology technology)
        {
            if (ModelState.IsValid)
            {
                _context.Add(technology);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(technology);
        }

        // GET: Technologies/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var technology = await _context.Technology.FindAsync(id);
            if (technology == null)
            {
                return NotFound();
            }
            return View(technology);
        }

        // POST: Technologies/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("technologyID,name,description")] Technology technology)
        {
            if (id != technology.technologyID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(technology);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TechnologyExists(technology.technologyID))
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
            return View(technology);
        }

        // GET: Technologies/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var technology = await _context.Technology
                .FirstOrDefaultAsync(m => m.technologyID == id);
            if (technology == null)
            {
                return NotFound();
            }

            return View(technology);
        }

        // POST: Technologies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var technology = await _context.Technology.FindAsync(id);
            _context.Technology.Remove(technology);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TechnologyExists(int id)
        {
            return _context.Technology.Any(e => e.technologyID == id);
        }
    }
}
