using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GibJohnWebsite.Data;
using GibJohnWebsite.Models;

namespace GibJohnWebsite.Views
{
    public class YourLessonsController : Controller
    {
        private readonly GibJohnWebsiteContext _context;

        public YourLessonsController(GibJohnWebsiteContext context)
        {
            _context = context;
        }

        // GET: YourLessons
        public async Task<IActionResult> Index()
        {
            return View(await _context.YourLessons.ToListAsync());
        }

        // GET: YourLessons/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var yourLessons = await _context.YourLessons
                .FirstOrDefaultAsync(m => m.Id == id);
            if (yourLessons == null)
            {
                return NotFound();
            }

            return View(yourLessons);
        }

        // GET: YourLessons/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: YourLessons/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Description,Time,Tutor")] YourLessons yourLessons)
        {
            if (ModelState.IsValid)
            {
                _context.Add(yourLessons);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(yourLessons);
        }

        // GET: YourLessons/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var yourLessons = await _context.YourLessons.FindAsync(id);
            if (yourLessons == null)
            {
                return NotFound();
            }
            return View(yourLessons);
        }

        // POST: YourLessons/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,Title,Description,Time,Tutor")] YourLessons yourLessons)
        {
            if (id != yourLessons.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(yourLessons);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!YourLessonsExists(yourLessons.Id))
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
            return View(yourLessons);
        }

        // GET: YourLessons/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var yourLessons = await _context.YourLessons
                .FirstOrDefaultAsync(m => m.Id == id);
            if (yourLessons == null)
            {
                return NotFound();
            }

            return View(yourLessons);
        }

        // POST: YourLessons/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var yourLessons = await _context.YourLessons.FindAsync(id);
            if (yourLessons != null)
            {
                _context.YourLessons.Remove(yourLessons);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool YourLessonsExists(string id)
        {
            return _context.YourLessons.Any(e => e.Id == id);
        }
    }
}
