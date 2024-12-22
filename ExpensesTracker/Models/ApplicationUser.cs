using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace ExpensesTracker.Models;

public class ApplicationUser : IdentityUser
{
	[StringLength(100, ErrorMessage = "{0} może mieć maksymalnie 100 znaków.")]
	[Display(Name = "Imię")]
	public string Name {get; set;}
	[StringLength(100, ErrorMessage = "{0} może mieć maksymalnie 100 znaków.")]
	[Display(Name = "Nazwisko")]
	public string Surname { get; set; }
}