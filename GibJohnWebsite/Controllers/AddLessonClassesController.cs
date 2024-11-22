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
    public class AddLessonClassesController : Controller
    {
        private readonly GibJohnWebsiteContext _context;

        public AddLessonClassesController(GibJohnWebsiteContext context)
        {
            _context = context;
        }

        // GET: AddLessonClasses
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> Index()
        {
            return View(await _context.AddLessonClass.ToListAsync());
        }

        // GET: AddLessonClasses/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var addLessonClass = await _context.AddLessonClass
                .FirstOrDefaultAsync(m => m.Id == id);
            if (addLessonClass == null)
            {
                return NotFound();
            }

            return View(addLessonClass);
        }

        // GET: AddLessonClasses/Create
        [Authorize(Roles = "Tutor")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: AddLessonClasses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Description,Time,Tutor")] AddLessonClass addLessonClass)
        {
            if (ModelState.IsValid)
            {
                _context.Add(addLessonClass);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(addLessonClass);
        }

        // GET: AddLessonClasses/Edit/5
        [Authorize(Roles = "Tutor")]
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var addLessonClass = await _context.AddLessonClass.FindAsync(id);
            if (addLessonClass == null)
            {
                return NotFound();
            }
            return View(addLessonClass);
        }

        // POST: AddLessonClasses/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Edit(string id, [Bind("Id,Title,Description,Time,Tutor")] AddLessonClass addLessonClass)
        {
            if (id != addLessonClass.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(addLessonClass);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AddLessonClassExists(addLessonClass.Id))
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
            return View(addLessonClass);
        }

        // GET: AddLessonClasses/Delete/5
        [Authorize(Roles = "Tutor")]
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var addLessonClass = await _context.AddLessonClass
                .FirstOrDefaultAsync(m => m.Id == id);
            if (addLessonClass == null)
            {
                return NotFound();
            }

            return View(addLessonClass);
        }

        // POST: AddLessonClasses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var addLessonClass = await _context.AddLessonClass.FindAsync(id);
            if (addLessonClass != null)
            {
                _context.AddLessonClass.Remove(addLessonClass);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AddLessonClassExists(string id)
        {
            return _context.AddLessonClass.Any(e => e.Id == id);
        }

        
    }
}