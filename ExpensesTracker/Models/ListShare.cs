namespace ExpensesTracker.Models;

public class ListShare
{
	public int Id { get; set; }
	public int ListName { get; set; }
	public List List { get; set; }
	public string UserId { get; set; }
	public ApplicationUser User { get; set; }
}