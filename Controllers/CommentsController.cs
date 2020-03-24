using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Jobbsy.Data;
using Jobbsy.Models;
using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Jobbsy.Controllers
{
    public class CommentsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CommentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Comments
        public async Task<IActionResult> Index()
        {
            TempData.Keep();
            return View(await _context.Comment.ToListAsync());
        }

        public async Task<IActionResult> CommentsForCompanyWithGivenID(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var company = await _context.Company.Include(com => com.CommentCollection).FirstOrDefaultAsync(company => company.companyID == id);

            if (company == null)
            {
                return NotFound();
            }
            TempData.Keep();
            return View("Index", company);
        }

        // GET: Comments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var comment = await _context.Comment
                .FirstOrDefaultAsync(m => m.commentID == id);
            if (comment == null)
            {
                return NotFound();
            }

            return View(comment);
        }



        [Authorize(Roles = "NormalUser")]
        public IActionResult Create()
        {
            TempData.Keep(); 
            return View();
        }

        // POST: Comments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("commentID,AspNetUsersID,rate,content,date")] Comment comment, int? id)
        {
            comment.date = DateTime.Now.ToString("MM/dd/yyyy");
            int companyID = id ?? 1;
            TempData["companyID"] = companyID;

            if (ModelState.IsValid)
            {
                var company = await _context.Company
                .Where(company => company.companyID == companyID)
                .Include(p => p.PhotoCollection)
                .Include(tc => tc.TechnologyCompany)
                .ThenInclude(tc => tc.Technology)
                .Include(com => com.CommentCollection)
                .FirstOrDefaultAsync<Company>();

                company.CommentCollection.Add(comment);
                _context.Update(company);
                await _context.SaveChangesAsync();

                return RedirectToAction("CommentsForCompanyWithGivenID", new { id = companyID });
            }
            return RedirectToAction("CommentsForCompanyWithGivenID", new { id = companyID });
        }

        // GET: Comments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var comment = await _context.Comment.FindAsync(id);
            if (comment == null)
            {
                return NotFound();
            }
            return View(comment);
        }

        // POST: Comments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("commentID,AspNetUsersID,rate,content,date")] Comment comment)
        {
            if (id != comment.commentID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(comment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CommentExists(comment.commentID))
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
            return View(comment);
        }

        // GET: Comments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var comment = await _context.Comment
                .FirstOrDefaultAsync(m => m.commentID == id);
            if (comment == null)
            {
                return NotFound();
            }

            return View(comment);
        }

        // POST: Comments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var comment = await _context.Comment.FindAsync(id);
            _context.Comment.Remove(comment);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CommentExists(int id)
        {
            return _context.Comment.Any(e => e.commentID == id);
        }
    }
}
