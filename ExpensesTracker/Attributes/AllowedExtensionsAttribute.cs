using System.ComponentModel.DataAnnotations;

namespace ExpensesTracker.Attributes;

public class AllowedExtensionsAttribute : ValidationAttribute
{
	private readonly string[] _allowedExtensions;

	public AllowedExtensionsAttribute(string[] allowedExtensions)
	{
		_allowedExtensions = allowedExtensions;
	}

	protected override ValidationResult IsValid(object value, ValidationContext validationContext)
	{
		var file = value as IFormFile;
		if (file != null)
		{
			var extension = Path.GetExtension(file.FileName);
			if (!_allowedExtensions.Contains(extension.ToLower()))
			{
				return new ValidationResult(ErrorMessage);
			}
		}
		
		return ValidationResult.Success;
	}

	public string GetErrorMessage()
	{
		return this.ErrorMessage;
	}
}