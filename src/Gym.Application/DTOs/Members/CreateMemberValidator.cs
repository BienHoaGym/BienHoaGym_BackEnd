using FluentValidation;
using Gym.Application.DTOs.Members;

namespace Gym.Application.Validators.Members;

public class CreateMemberValidator : AbstractValidator<CreateMemberDto>
{
    public CreateMemberValidator()
    {
        RuleFor(x => x.FullName)
            .NotEmpty().WithMessage("H? Tên không du?c d? tr?ng")
            .MaximumLength(50).WithMessage("H? TênTên không quá 50 ký t?");


        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email là b?t bu?c")
            .EmailAddress().WithMessage("Ð?nh d?ng email không h?p l?");

        RuleFor(x => x.PhoneNumber)
            .NotEmpty().WithMessage("S? di?n tho?i là b?t bu?c")
            .Matches(@"^\d{10,11}$").WithMessage("S? di?n tho?i không h?p l? (ph?i là 10-11 s?)");

        RuleFor(x => x.DateOfBirth)
            .LessThan(DateTime.UtcNow.AddYears(-10)).WithMessage("H?i viên ph?i trên 10 tu?i");
    }
}
