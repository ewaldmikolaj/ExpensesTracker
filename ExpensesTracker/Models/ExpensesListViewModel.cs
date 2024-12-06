namespace ExpensesTracker.Models;

public class ExpensesListViewModel
{
	public List List { get; set; }
	public string UserId { get; set; }
	public IEnumerable<Expense> Expenses { get; set; }
}