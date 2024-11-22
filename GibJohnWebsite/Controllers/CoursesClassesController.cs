using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GibJohnWebsite.Data;
using GibJohnWebsite.Models;
using Microsoft.AspNetCore.Authorization;

namespace GibJohnWebsite.Controllers
{
    public class CoursesClassesController : Controller
    {
        private readonly GibJohnWebsiteContext _context;

        public CoursesClassesController(GibJohnWebsiteContext context)
        {
            _context = context;
        }

        // GET: CoursesClasses
        [Authorize(Roles ="Student")]
        public async Task<IActionResult> Index()
        {
            return View(await _context.CoursesClass.ToListAsync());
        }

        // GET: CoursesClasses/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var coursesClass = await _context.CoursesClass
                .FirstOrDefaultAsync(m => m.Id == id);
            if (coursesClass == null)
            {
                return NotFound();
            }

            return View(coursesClass);
        }

        // GET: CoursesClasses/Create
        [Authorize(Roles ="Admin")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: CoursesClasses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Description")] CoursesClass coursesClass)
        {
            if (ModelState.IsValid)
            {
                _context.Add(coursesClass);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(coursesClass);
        }

        // GET: CoursesClasses/Edit/5
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var coursesClass = await _context.CoursesClass.FindAsync(id);
            if (coursesClass == null)
            {
                return NotFound();
            }
            return View(coursesClass);
        }

        // POST: CoursesClasses/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,Title,Description")] CoursesClass coursesClass)
        {
            if (id != coursesClass.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(coursesClass);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CoursesClassExists(coursesClass.Id))
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
            return View(coursesClass);
        }

        // GET: CoursesClasses/Delete/5
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var coursesClass = await _context.CoursesClass
                .FirstOrDefaultAsync(m => m.Id == id);
            if (coursesClass == null)
            {
                return NotFound();
            }

            return View(coursesClass);
        }

        // POST: CoursesClasses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var coursesClass = await _context.CoursesClass.FindAsync(id);
            if (coursesClass != null)
            {
                _context.CoursesClass.Remove(coursesClass);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CoursesClassExists(string id)
        {
            return _context.CoursesClass.Any(e => e.Id == id);
        }
    }
}
