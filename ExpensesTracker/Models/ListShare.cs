namespace ExpensesTracker.Models;

public class ListShare
{
	public int Id { get; set; }
	public int? ListId { get; set; }
	public virtual List? List { get; set; }
	public string? UserId { get; set; }
	public virtual ApplicationUser? User { get; set; }
}