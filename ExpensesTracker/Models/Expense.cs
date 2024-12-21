using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace ExpensesTracker.Models;

public class Expense
{
	public int Id { get; set; }
	[Required(ErrorMessage = "{0} jest wymagany.")]
	[StringLength(100, ErrorMessage = "{0} może mieć maksymalnie 100 znaków.")]
	[Display(Name = "Tytuł")]
	public string Title { get; set; }
	[Required(ErrorMessage = "{0} jest wymagana.")]
	[DataType(DataType.DateTime, ErrorMessage = "Wartość musi być poprawną datą.")]
	[DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm}")]
	[Display(Name = "Data")]
	public DateTime? Date { get; set; }
	[Required(ErrorMessage = "{0} jest wymagana.")]
	[DataType(DataType.Currency, ErrorMessage = "Wartość {0} nie jest prawidłową kwotą.")]
	[Range(0, double.MaxValue, ErrorMessage = "{0} musi być większa od zera.")]
	[Display(Name = "Kwota")]
	public decimal? Amount { get; set; }
	[Display(Name = "Płatnik")]
	public string? PayerId { get; set; }
	public virtual ApplicationUser? Payer { get; set; }
	[Display(Name = "Zdjęcie")]
	public int? ReceiptPhotoId { get; set; }
	public virtual ReceiptPhoto? ReceiptPhoto { get; set; }
	[Display(Name = "Lista")]
	public int? ListId {get; set;}
	public virtual List? List { get; set; }
}