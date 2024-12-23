using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ExpensesTracker.Data;
using ExpensesTracker.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Data.SqlClient;
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

        private async Task<ListSharesViewModel?> CreateListShareViewModel(int listId)
        {
            var list = await _context.List.FindAsync(listId);
            if (list == null) return null;
            var listShares = await _context.ListShare
                .Include(l => l.User)
                .Where(l => l.ListId == listId)
                .ToListAsync();

            return new ListSharesViewModel
            {
                List = list,
                ListShares = listShares
            };
        }

        // GET: ListShare
        public async Task<IActionResult> Index([FromRoute] int listId)
        {
            var listShareViewModel = await CreateListShareViewModel(listId);
            if (listShareViewModel == null)
            {
                return NotFound();
            }
            
            return View(listShareViewModel);
        }

        // POST: ListShare/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int listId, string? userEmail)
        {
            var currentUser = User.FindFirstValue(ClaimTypes.NameIdentifier); 
            var list = await _context.List.FindAsync(listId);
            var user = _context.Users.FirstOrDefault(u => u.Email == userEmail);
            
            if (list == null)
            {
                return NotFound();
            }
            
            if (currentUser != list.OwnerId)
            {
                return Unauthorized();
            }

            if (string.IsNullOrEmpty(userEmail))
            {
                ModelState.AddModelError("userEmail", "Email jest wymagany."); 
            }
            else if (user == null)
            {
                ModelState.AddModelError("userEmail", "Dany użytkownik nie istnieje.");
            } 
            else if (user.Id == list.OwnerId)
            {
                ModelState.AddModelError("userEmail", "Nie można udostępnić listy jej właścicielowi");
            }
            
            if (ModelState.IsValid)
            {
                try
                {
                    var listShare = new ListShare
                    {
                        ListId = list.Id,
                        UserId = user.Id,
                    };
                    
                    _context.Add(listShare);
                    await _context.SaveChangesAsync();
                    return RedirectToRoute("ListShare", new { listId = list.Id });
                }
                catch (DbUpdateException exception)
                {
                    ModelState.AddModelError("userEmail", "Podany email posiada już dostęp do listy");
                }
            }
            
            var viewModel = await CreateListShareViewModel(listId);
            return View("Index", viewModel);
        }

        // POST: ListShare/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id, int listId)
        {
            var currentUser = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var list = await _context.List.FindAsync(listId);
            var listShare = await _context.ListShare.FindAsync(id);

            if (currentUser != list.OwnerId)
            {
                return Unauthorized();
            }
            
            if (listShare != null)
            {
                var expenses = await _context.Expense
                    .Where(e => e.ListId == listId && e.PayerId == listShare.UserId)
                    .ToListAsync();
                foreach (var expense in expenses)
                {
                    expense.ListId = null;
                }
                
                _context.ListShare.Remove(listShare);
                await _context.SaveChangesAsync();
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
