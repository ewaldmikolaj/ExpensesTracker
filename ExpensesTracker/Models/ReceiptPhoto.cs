using System.ComponentModel.DataAnnotations;

namespace ExpensesTracker.Models;

public class ReceiptPhoto
{    
    public Guid Id { get; set; } = Guid.NewGuid();
    [StringLength(60)]
    public required string Path { get; set; }
    public Expense? Expense { get; set; }
}