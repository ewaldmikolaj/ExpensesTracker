using System.ComponentModel.DataAnnotations;

namespace ExpensesTracker.Models;

public class List
{
	public int Id { get; set; }
	[Display(Name = "Nazwa")]
	public string Name { get; set; }
	public string? OwnerId {get; set;}
	public virtual ApplicationUser? Owner { get; set; }
}