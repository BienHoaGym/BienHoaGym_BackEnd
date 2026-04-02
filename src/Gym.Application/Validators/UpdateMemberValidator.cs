using FluentValidation;
using Gym.Application.DTOs.Members;

namespace Gym.Application.Validators.Members;

public class UpdateMemberValidator : AbstractValidator<UpdateMemberDto>
{
    public UpdateMemberValidator()
    {
        // 1. Ki?m tra FirstName (Thay vì FullName)
        RuleFor(x => x.FullName)
           .NotEmpty().WithMessage("Full Name is required")
           .MaximumLength(100).WithMessage("Full Name must not exceed 100 characters");

        
        // 3. Ki?m tra Email
        RuleFor(x => x.Email)
            .EmailAddress().When(x => !string.IsNullOrEmpty(x.Email))
            .WithMessage("Invalid email format");

        // 4. Ki?m tra PhoneNumber
        RuleFor(x => x.PhoneNumber)
            .NotEmpty().WithMessage("Phone number is required")
            .Matches(@"^0\d{9}$").WithMessage("Phone number must be 10 digits starting with 0");

        // 5. Ki?m tra ngày sinh (DateOfBirth)
        RuleFor(x => x.DateOfBirth)
            .LessThan(DateTime.Today).When(x => x.DateOfBirth.HasValue)
            .WithMessage("Date of birth must be in the past")
            .Must(BeAtLeast10YearsOld).When(x => x.DateOfBirth.HasValue)
            .WithMessage("Member must be at least 10 years old");

        // 6. Ki?m tra gi?i tính (Gender)
        RuleFor(x => x.Gender)
            .Must(g => string.IsNullOrEmpty(g) || new[] { "Male", "Female", "Other" }.Contains(g))
            .WithMessage("Gender must be Male, Female, or Other");

        // 7. Ki?m tra s? di?n tho?i kh?n c?p
        RuleFor(x => x.PhoneNumber)
            .Matches(@"^0\d{9}$").When(x => !string.IsNullOrEmpty(x.PhoneNumber))
            .WithMessage("Emergency phone must be 10 digits starting with 0");
    }

    private bool BeAtLeast10YearsOld(DateTime? dateOfBirth)
    {
        if (!dateOfBirth.HasValue) return true;
        var age = DateTime.Today.Year - dateOfBirth.Value.Year;
        if (dateOfBirth.Value.Date > DateTime.Today.AddYears(-age)) age--;
        return age >= 10;
    }
}
