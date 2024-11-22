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
    public class TutorsClassesController : Controller
    {
        private readonly GibJohnWebsiteContext _context;

        public TutorsClassesController(GibJohnWebsiteContext context)
        {
            _context = context;
        }

        // GET: TutorsClasses
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> Index()
        {
            return View(await _context.TutorsClass.ToListAsync());
        }

        // GET: TutorsClasses/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tutorsClass = await _context.TutorsClass
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tutorsClass == null)
            {
                return NotFound();
            }

            return View(tutorsClass);
        }

        // GET: TutorsClasses/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: TutorsClasses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Email,PhoneNumber,Subject")] TutorsClass tutorsClass)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tutorsClass);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(tutorsClass);
        }

        // GET: TutorsClasses/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tutorsClass = await _context.TutorsClass.FindAsync(id);
            if (tutorsClass == null)
            {
                return NotFound();
            }
            return View(tutorsClass);
        }

        // POST: TutorsClasses/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,Name,Email,PhoneNumber,Subject")] TutorsClass tutorsClass)
        {
            if (id != tutorsClass.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tutorsClass);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TutorsClassExists(tutorsClass.Id))
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
            return View(tutorsClass);
        }

        // GET: TutorsClasses/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tutorsClass = await _context.TutorsClass
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tutorsClass == null)
            {
                return NotFound();
            }

            return View(tutorsClass);
        }

        // POST: TutorsClasses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var tutorsClass = await _context.TutorsClass.FindAsync(id);
            if (tutorsClass != null)
            {
                _context.TutorsClass.Remove(tutorsClass);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TutorsClassExists(string id)
        {
            return _context.TutorsClass.Any(e => e.Id == id);
        }
    }
}
