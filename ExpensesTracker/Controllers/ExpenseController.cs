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
    public class ExpenseController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ExpenseController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Expense
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Expense.Include(e => e.Payer);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Expense/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var expense = await _context.Expense
                .Include(e => e.Payer)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (expense == null)
            {
                return NotFound();
            }

            return View(expense);
        }

        // GET: Expense/Create
        public IActionResult Create()
        {
            ViewData["PayerId"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        // POST: Expense/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Date,Amount,PayerId,ReceiptPhotoId,ListId")] Expense expense)
        {
            if (ModelState.IsValid)
            {
                _context.Add(expense);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["PayerId"] = new SelectList(_context.Users, "Id", "Id", expense.PayerId);
            return View(expense);
        }

        // GET: Expense/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var expense = await _context.Expense.FindAsync(id);
            if (expense == null)
            {
                return NotFound();
            }
            ViewData["PayerId"] = new SelectList(_context.Users, "Id", "Id", expense.PayerId);
            return View(expense);
        }

        // POST: Expense/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Date,Amount,PayerId,ReceiptPhotoId,ListId")] Expense expense)
        {
            if (id != expense.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(expense);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ExpenseExists(expense.Id))
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
            ViewData["PayerId"] = new SelectList(_context.Users, "Id", "Id", expense.PayerId);
            return View(expense);
        }

        // GET: Expense/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var expense = await _context.Expense
                .Include(e => e.Payer)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (expense == null)
            {
                return NotFound();
            }

            return View(expense);
        }

        // POST: Expense/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var expense = await _context.Expense.FindAsync(id);
            if (expense != null)
            {
                _context.Expense.Remove(expense);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ExpenseExists(int id)
        {
            return _context.Expense.Any(e => e.Id == id);
        }
    }
}
