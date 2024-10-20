using Microsoft.EntityFrameworkCore;

namespace ExpensesTracker.Models;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) {}

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        
        builder.HasDefaultSchema("application");
        builder.Entity<ReceiptPhoto>()
            .HasOne(e => e.Expense)
            .WithOne(e => e.ReceiptPhoto)
            .HasForeignKey<Expense>(e => e.ReceiptPhotoId)
            .IsRequired(false);
    }
    
    public DbSet<ReceiptPhoto> ReceiptPhotos { get; set; }
    public DbSet<Expense> Expenses { get; set; }
}