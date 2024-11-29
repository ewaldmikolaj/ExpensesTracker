using System.ComponentModel.DataAnnotations;

namespace ExpensesTracker.Models;

public class List
{
	public int Id { get; set; }
	[Display(Name = "Nazwa")]
	public string Name { get; set; }
	[Display(Name = "Czy publiczna")]
	public bool IsPublic { get; set; }
	[Display(Name = "Link publiczny")]
	public string? PublicUrl { get; set; }
	public string? OwnerId {get; set;}
	public virtual ApplicationUser? Owner { get; set; }
}