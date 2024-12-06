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

namespace ExpensesTracker
{
    [Authorize]
    public class ListController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ListController(ApplicationDbContext context)
        {
            _context = context;
        }

        private async Task<ListsViewModel> GetAvailableLists()
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
            var lists = new ListsViewModel
            {
                OwnedLists = ownedLists,
                SharedLists = sharedLists
            };
            
            return lists;
        }

        // GET: List
        public async Task<IActionResult> Index()
        {
            return View(await GetAvailableLists());
        }

        // GET: List/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            
            var availableLists = await GetAvailableLists();
            var list = availableLists.OwnedLists?.FirstOrDefault(ls => ls.Id == id) ??
                       availableLists.SharedLists?.FirstOrDefault(ls => ls.Id == id);
            
            if (list == null)
            {
                return NotFound();
            }
            
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var expenses = await _context.Expense
                .Where(e => e.ListId == id)
                .ToListAsync();

            var expensesList = new ExpensesListViewModel
            {
                List = list,
                UserId = userId,
                Expenses = expenses
            };

            return View(expensesList);
        }

        // GET: List/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: List/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(List list)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            list.OwnerId = userId;
            
            if (ModelState.IsValid)
            {
                _context.Add(list);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            
            return View(list);
        }

        // GET: List/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            
            if (id == null)
            {
                return NotFound();
            }

            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var list = await _context.List
                .Include(l => l.Owner)
                .Where(l => l.OwnerId == userId)
                .FirstOrDefaultAsync(m => m.Id == id);
            
            if (list == null)
            {
                return NotFound();
            }
            
            return View(list);
        }

        // POST: List/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, List list)
        {
            if (id != list.Id)
            {
                return NotFound();
            }
            
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (ModelState.IsValid)
            {
                try
                {
                    var existingList = await _context.List
                        .Include(l => l.Owner)
                        .Where(l => l.OwnerId == userId)
                        .FirstOrDefaultAsync(m => m.Id == id);

                    if (existingList == null)
                    {
                        return NotFound();
                    }
                    
                    existingList.Name = list.Name;
                    existingList.IsPublic = list.IsPublic;
                    existingList.PublicUrl = list.PublicUrl;
                    
                    _context.Update(existingList);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ListExists(list.Id))
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

            return View(list);
        }

        // GET: List/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var list = await _context.List
                .Include(l => l.Owner)
                .FirstOrDefaultAsync(m => m.Id == id);
            
            if (list == null)
            {
                return NotFound();
            }

            return View(list);
        }

        // POST: List/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var list = await _context.List
                .Include(l => l.Owner)
                .Where(l => l.OwnerId == userId)
                .FirstOrDefaultAsync(m => m.Id == id);
            
            if (list != null)
            {
                _context.List.Remove(list);
            }
            await _context.SaveChangesAsync();
            
            return RedirectToAction(nameof(Index));
        }

        private bool ListExists(int id)
        {
            return _context.List.Any(e => e.Id == id);
        }
    }
}
