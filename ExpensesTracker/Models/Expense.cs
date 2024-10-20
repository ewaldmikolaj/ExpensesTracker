using System.ComponentModel.DataAnnotations;

namespace ExpensesTracker.Models;

public class Expense
{
    public Guid Id { get; set; } = Guid.NewGuid();
    [StringLength(60)]
    public required string Name { get; set; }
    public decimal Amount { get; set; }
    public Guid? ReceiptPhotoId { get; set; }
    public ReceiptPhoto? ReceiptPhoto { get; set; }
}