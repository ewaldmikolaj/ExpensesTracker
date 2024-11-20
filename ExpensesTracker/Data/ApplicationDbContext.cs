using ExpensesTracker.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ExpensesTracker.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
	public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
		: base(options)
	{
	}

public DbSet<ExpensesTracker.Models.List> List { get; set; } = default!;

public DbSet<ExpensesTracker.Models.Expense> Expense { get; set; } = default!;

public DbSet<ExpensesTracker.Models.ListShare> ListShare { get; set; } = default!;

public DbSet<ExpensesTracker.Models.ReceiptPhoto> ReceiptPhoto { get; set; } = default!;
}