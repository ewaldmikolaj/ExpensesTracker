using System.ComponentModel.DataAnnotations.Schema;
using ExpensesTracker.Attributes;

namespace ExpensesTracker.Models;

public class ReceiptPhoto
{
	public int Id { get; set; }
	public string? Path { get; set; }
	[NotMapped]
	[AllowedExtensions(new string[] { ".jpg", ".jpeg", ".png" }, ErrorMessage = "Plik musi być zdjęciem.")]
	public IFormFile Photo { get; set; }
}