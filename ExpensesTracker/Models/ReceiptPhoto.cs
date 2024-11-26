using System.ComponentModel.DataAnnotations.Schema;

namespace ExpensesTracker.Models;

public class ReceiptPhoto
{
	public int Id { get; set; }
	public string? Path { get; set; }
	[NotMapped]
	public IFormFile Photo { get; set; }
}