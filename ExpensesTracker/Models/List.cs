namespace ExpensesTracker.Models;

public class List
{
	public int Id { get; set; }
	public string Name { get; set; }
	public bool IsPublic { get; set; }
	public string? PublicUrl { get; set; }
	public string OwnerId {get; set;}
	public ApplicationUser? Owner { get; set; }
}