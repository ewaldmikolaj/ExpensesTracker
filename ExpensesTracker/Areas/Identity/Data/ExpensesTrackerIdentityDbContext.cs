using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ExpensesTracker.Areas.Identity.Data;

public class ExpensesTrackerIdentityDbContext : IdentityDbContext<IdentityUser>
{
    public ExpensesTrackerIdentityDbContext(DbContextOptions<ExpensesTrackerIdentityDbContext> options) : base(options) { }
    
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.HasDefaultSchema("identity");
    }
}
