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
using NuGet.Protocol;

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
        public async Task<IActionResult> Index([FromRoute] int listId)
        {
            var list = await _context.List.FindAsync(listId);
            if (list == null)
            {
                return NotFound();
            }
            
            var listShares = await _context.ListShare
                .Include(l => l.User)
                .Where(l => l.ListId == listId)
                .ToListAsync();

            var listShareViewModel = new ListSharesViewModel
            {
                List = list,
                ListShares = listShares
            };
            
            return View(listShareViewModel);
        }

        // POST: ListShare/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int listId, string userEmail)
        {
            var list = await _context.List.FindAsync(listId);
            var user = _context.Users.FirstOrDefault(u => u.Email == userEmail);
            
            if (list == null || user == null)
            {
                return NotFound();
            }

            var listShare = new ListShare
            {
                ListId = list.Id,
                UserId = user.Id,
            };
            
            if (ModelState.IsValid)
            {
                _context.Add(listShare);
                await _context.SaveChangesAsync();
                return RedirectToRoute("ListShare", new { listId = list.Id });
            }
            
            return RedirectToRoute("ListShare", new { listId = list.Id });
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

        // POST: ListShare/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id, int listId)
        {
            var listShare = await _context.ListShare.FindAsync(id);
            
            if (listShare != null)
            {
                _context.ListShare.Remove(listShare);
            }

            await _context.SaveChangesAsync();
            return RedirectToRoute("ListShare", new { listId = listId });
        }

        private bool ListShareExists(int id)
        {
            return _context.ListShare.Any(e => e.Id == id);
        }
    }
}
