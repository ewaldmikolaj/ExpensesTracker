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
    public class ReceiptPhotoController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ReceiptPhotoController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ReceiptPhoto
        public async Task<IActionResult> Index()
        {
            return View(await _context.ReceiptPhoto.ToListAsync());
        }

        // GET: ReceiptPhoto/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var receiptPhoto = await _context.ReceiptPhoto
                .FirstOrDefaultAsync(m => m.Id == id);
            if (receiptPhoto == null)
            {
                return NotFound();
            }

            return View(receiptPhoto);
        }

        // GET: ReceiptPhoto/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ReceiptPhoto/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Path")] ReceiptPhoto receiptPhoto)
        {
            if (ModelState.IsValid)
            {
                _context.Add(receiptPhoto);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(receiptPhoto);
        }

        // GET: ReceiptPhoto/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var receiptPhoto = await _context.ReceiptPhoto.FindAsync(id);
            if (receiptPhoto == null)
            {
                return NotFound();
            }
            return View(receiptPhoto);
        }

        // POST: ReceiptPhoto/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Path")] ReceiptPhoto receiptPhoto)
        {
            if (id != receiptPhoto.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(receiptPhoto);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReceiptPhotoExists(receiptPhoto.Id))
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
            return View(receiptPhoto);
        }

        // GET: ReceiptPhoto/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var receiptPhoto = await _context.ReceiptPhoto
                .FirstOrDefaultAsync(m => m.Id == id);
            if (receiptPhoto == null)
            {
                return NotFound();
            }

            return View(receiptPhoto);
        }

        // POST: ReceiptPhoto/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var receiptPhoto = await _context.ReceiptPhoto.FindAsync(id);
            if (receiptPhoto != null)
            {
                _context.ReceiptPhoto.Remove(receiptPhoto);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ReceiptPhotoExists(int id)
        {
            return _context.ReceiptPhoto.Any(e => e.Id == id);
        }
    }
}
