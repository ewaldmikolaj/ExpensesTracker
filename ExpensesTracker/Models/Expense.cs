using System.ComponentModel.DataAnnotations;

namespace ExpensesTracker.Models;

public class Expense
{
	public int Id { get; set; }
	[Display(Name = "Tytuł")]
	public string Title { get; set; }
	[Display(Name = "Data")]
	public DateTime Date { get; set; }
	[Display(Name = "Kwota")]
	public decimal Amount { get; set; }
	[Display(Name = "Płatnik")]
	public string? PayerId { get; set; }
	public ApplicationUser? Payer { get; set; }
	[Display(Name = "Zdjęcie")]
	public int? ReceiptPhotoId { get; set; }
	public ReceiptPhoto? ReceiptPhoto { get; set; }
	public int ListId {get; set;}
	public List List { get; set; }
}