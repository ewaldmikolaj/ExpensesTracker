using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ExpensesTracker.Data;
using ExpensesTracker.Models;
using Microsoft.AspNetCore.Authorization;

namespace ExpensesTracker.Controllers
{
    [Authorize]
    public class ListShareController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ListShareController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ListShare
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.ListShare.Include(l => l.User);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: ListShare/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var listShare = await _context.ListShare
                .Include(l => l.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (listShare == null)
            {
                return NotFound();
            }

            return View(listShare);
        }

        // GET: ListShare/Create
        public IActionResult Create()
        {
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        // POST: ListShare/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ListName,UserId")] ListShare listShare)
        {
            if (ModelState.IsValid)
            {
                _context.Add(listShare);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", listShare.UserId);
            return View(listShare);
        }

        // GET: ListShare/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var listShare = await _context.ListShare.FindAsync(id);
            if (listShare == null)
            {
                return NotFound();
            }
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", listShare.UserId);
            return View(listShare);
        }

        // POST: ListShare/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ListName,UserId")] ListShare listShare)
        {
            if (id != listShare.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(listShare);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ListShareExists(listShare.Id))
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
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", listShare.UserId);
            return View(listShare);
        }

        // GET: ListShare/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var listShare = await _context.ListShare
                .Include(l => l.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (listShare == null)
            {
                return NotFound();
            }

            return View(listShare);
        }

        // POST: ListShare/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var listShare = await _context.ListShare.FindAsync(id);
            if (listShare != null)
            {
                _context.ListShare.Remove(listShare);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ListShareExists(int id)
        {
            return _context.ListShare.Any(e => e.Id == id);
        }
    }
}
