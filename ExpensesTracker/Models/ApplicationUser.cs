using Microsoft.AspNetCore.Identity;

namespace ExpensesTracker.Models;

public class ApplicationUser : IdentityUser
{
	public string Name {get; set;}
	public string Surname { get; set; }
}