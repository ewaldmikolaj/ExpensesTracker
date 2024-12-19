using Microsoft.AspNetCore.Identity;

namespace ExpensesTracker.Services;

public class CustomIdentityErrorDescriber : IdentityErrorDescriber
{
	    public override IdentityError DuplicateEmail(string email)
    {
        return new IdentityError
        {
            Code = nameof(DuplicateEmail),
            Description = $"Email '{email}' jest już zajęty."
        };
    }

    public override IdentityError DuplicateUserName(string userName)
    {
        return new IdentityError
        {
            Code = nameof(DuplicateUserName),
            Description = $"Nazwa użytkownika '{userName}' jest już zajęta."
        };
    }

    public override IdentityError InvalidEmail(string email)
    {
        return new IdentityError
        {
            Code = nameof(InvalidEmail),
            Description = $"Email '{email}' nie jest prawidłowy."
        };
    }

    public override IdentityError PasswordRequiresNonAlphanumeric()
    {
        return new IdentityError
        {
            Code = nameof(PasswordRequiresNonAlphanumeric),
            Description = "Hasło musi zawierać co najmniej jeden znak niealfanumeryczny."
        };
    }

    public override IdentityError PasswordRequiresDigit()
    {
        return new IdentityError
        {
            Code = nameof(PasswordRequiresDigit),
            Description = "Hasło musi zawierać co najmniej jedną cyfrę ('0'-'9')."
        };
    }

    public override IdentityError PasswordRequiresLower()
    {
        return new IdentityError
        {
            Code = nameof(PasswordRequiresLower),
            Description = "Hasło musi zawierać co najmniej jedną małą literę ('a'-'z')."
        };
    }

    public override IdentityError PasswordRequiresUpper()
    {
        return new IdentityError
        {
            Code = nameof(PasswordRequiresUpper),
            Description = "Hasło musi zawierać co najmniej jedną wielką literę ('A'-'Z')."
        };
    }

    public override IdentityError PasswordTooShort(int length)
    {
        return new IdentityError
        {
            Code = nameof(PasswordTooShort),
            Description = $"Hasło musi zawierać co najmniej {length} znaków."
        };
    }
}