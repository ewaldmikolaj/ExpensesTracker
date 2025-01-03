using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ExpensesTracker.Data;
using ExpensesTracker.Models;
using Microsoft.AspNetCore.Authorization;
using NuGet.Packaging;
using NuGet.Protocol;

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
        
        private async Task<List<List>> GetAvailableLists()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var ownedLists = await _context.List
                .Where(l => l.OwnerId == userId)
                .ToListAsync();
            var sharedLists = await _context.ListShare
                .Include(ls => ls.List)
                .Where(ls => ls.UserId == userId)
                .Select(ls => ls.List)
                .ToListAsync();
            ownedLists.AddRange(sharedLists);
            
            return ownedLists;
        }

        private string GetReturnUrl()
        {
            string? returnUrl = HttpContext.Session.GetString("ReturnUrl");
            if (string.IsNullOrEmpty(returnUrl))
            {
                return Url.Action("Index", "Expense");
            }
            return returnUrl;
        }

        // GET: Expense
        public async Task<IActionResult> Index()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var applicationDbContext = _context.Expense
                .Include(e => e.Payer)
                .Where(e => e.PayerId == userId);
            
            HttpContext.Session.SetString("ReturnUrl", HttpContext.Request.Path);
            
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Expense/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var expense = await _context.Expense
                .Include(e => e.Payer)
                .Include(e => e.ReceiptPhoto)
                .Include(e => e.List)
                .FirstOrDefaultAsync(m => m.Id == id);
            
            if (expense == null)
            {
                return NotFound();
            }
            
            var share = await _context.ListShare.AnyAsync(ls => ls.ListId == expense.ListId && ls.UserId == userId); ;
            if (expense.PayerId != userId && !share && (expense.List != null && expense.List.OwnerId != userId))
            {
                return NotFound();
            }
            ViewBag.UserId = userId;
            ViewBag.ReturnUrl = GetReturnUrl();
            
            return View(expense);
        }

        // GET: Expense/Create
        [HttpGet]
        public async Task<IActionResult> Create(int? listId = null)
        {
            var lists = await GetAvailableLists();
            ViewData["ListId"] = new SelectList(lists, "Id", "Name", listId);
            ViewBag.ReturnUrl = GetReturnUrl();
            
            return View();
        }

        // POST: Expense/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Expense expense)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            expense.PayerId = userId;
            
            if (expense.ReceiptPhoto != null && expense.ReceiptPhoto.Photo != null && expense.ReceiptPhoto.Photo.Length > 0)
            {
                var fileExtension = Path.GetExtension(expense.ReceiptPhoto.Photo.FileName);
                var relativePath = Path.Combine("/images/receipts", Guid.NewGuid().ToString() + fileExtension);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot" + relativePath);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await expense.ReceiptPhoto.Photo.CopyToAsync(stream);
                }
            
                expense.ReceiptPhoto.Path = relativePath;
            }
            
            if (ModelState.IsValid)
            {
                _context.Add(expense);
                await _context.SaveChangesAsync();
                return Redirect(GetReturnUrl());
            }

            var lists = await GetAvailableLists();
            ViewData["ListId"] = new SelectList(lists, "Id", "Name", null);
            ViewBag.ReturnUrl = GetReturnUrl();
            
            return View(expense);
        }

        // GET: Expense/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var expense = await _context.Expense
                .Include(e => e.Payer)
                .Include(e => e.ReceiptPhoto)
                .Include(e => e.List)
                .Where(e => e.PayerId == userId)
                .FirstOrDefaultAsync(m => m.Id == id);
            
            if (expense == null)
            {
                return NotFound();
            }
            
            var lists = await GetAvailableLists();
            ViewData["ListId"] = new SelectList(lists, "Id", "Name", null);
            return View(expense);
        }

        // POST: Expense/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Expense expense)
        {
            
            if (id != expense.Id)
            {
                return NotFound();
            }
            
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            
            if (ModelState.IsValid)
            {
                try
                {
                    var existingExpense = await _context.Expense
                        .Include(e => e.ReceiptPhoto)
                        .Where(e => e.PayerId == userId)
                        .FirstOrDefaultAsync(e => e.Id == expense.Id);

                    if (existingExpense == null)
                    {
                        return NotFound();
                    }
                    
                    existingExpense.Title = expense.Title;
                    existingExpense.Date = expense.Date;
                    existingExpense.Amount = expense.Amount;
                    existingExpense.ListId = expense.ListId;
                    
                    _context.Update(existingExpense);
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
                return Redirect(GetReturnUrl());;
            }
            
            var lists = _context.List.Include(l => l.Owner)
                .Where(l => l.OwnerId == userId);
            ViewData["ListId"] = new SelectList(lists, "Id", "Name", null);
            
            return View(expense);
        }

        // GET: Expense/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var expense = await _context.Expense
                .Include(e => e.Payer)
                .Include(e => e.ReceiptPhoto)
                .Include(e => e.List)
                .Where(e => e.PayerId == userId)
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
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var expense = await _context.Expense
                .Where(e => e.PayerId == userId)
                .FirstOrDefaultAsync(e => e.Id == id);
            
            if (expense != null)
            {
                _context.Expense.Remove(expense);
            }
            await _context.SaveChangesAsync();
            
            return Redirect(GetReturnUrl());;
        }
        
        // Receipt Photo related methods
        [HttpPost, ActionName("DeleteReceiptPhoto")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteReceiptPhoto(int expenseId)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var expense = await _context.Expense
                .Include(e => e.ReceiptPhoto)
                .Where(e => e.PayerId == userId)
                .FirstOrDefaultAsync(e => e.Id == expenseId);
            
            if (expense == null)
            {
                return NotFound();
            }

            if (expense.ReceiptPhoto != null)
            {
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot" + expense.ReceiptPhoto.Path);
                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }
                
                _context.ReceiptPhoto.Remove(expense.ReceiptPhoto);
                expense.ReceiptPhoto = null;
                await _context.SaveChangesAsync();
            }
            
            return RedirectToAction("Edit", new { id = expenseId });
        }

        [HttpPost, ActionName("AddReceiptPhoto")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddReceiptPhoto(int expenseId, IFormFile? photo)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var expense = await _context.Expense
                .Where(e => e.PayerId == userId)
                .FirstOrDefaultAsync(e => e.Id == expenseId);
            
            if (expense == null)
            {
                return NotFound();
            }
            
            if (photo != null && photo.Length > 0)
            {
                var fileExtension = Path.GetExtension(photo.FileName);
                var relativePath = Path.Combine("/images/receipts", Guid.NewGuid().ToString() + fileExtension);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot" + relativePath);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await photo.CopyToAsync(stream);
                }

                var receiptPhoto = new ReceiptPhoto();
                receiptPhoto.Path = relativePath;
                expense.ReceiptPhoto = receiptPhoto;
            }

            if (ModelState.IsValid)
            {
                _context.Update(expense);
                await _context.SaveChangesAsync();
            }
            
            return RedirectToAction("Edit", new { id = expenseId });
        }

        private bool ExpenseExists(int id)
        {
            return _context.Expense.Any(e => e.Id == id);
        }
    }
}
