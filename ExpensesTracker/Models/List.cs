using System.ComponentModel.DataAnnotations;
using Microsoft.VisualBasic;

namespace ExpensesTracker.Models;

public class List
{
	public int Id { get; set; }
	[Required(ErrorMessage = "{0} jest wymagana.")]
	[StringLength(100, ErrorMessage = "{0} może mieć maksymalnie 100 znaków.")]
	[Display(Name = "Nazwa")]
	public string Name { get; set; }
	public string? OwnerId {get; set;}
	public virtual ApplicationUser? Owner { get; set; }
}