using System;
using System.Collections.Generic;
using System.Text;
using FluentValidation;
using Gym.Application.DTOs.Members;

namespace Gym.Application.Validators;

public class CreateMemberValidator : AbstractValidator<CreateMemberDto>
{
    public CreateMemberValidator()
    {
        RuleFor(x => x.FullName)
            .NotEmpty().WithMessage("H? và tên không du?c d? tr?ng")
            .MaximumLength(100).WithMessage("H? và tên không quá 100 ký t?");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email là b?t bu?c")
            .EmailAddress().WithMessage("Email không dúng d?nh d?ng");

        RuleFor(x => x.PhoneNumber)
            .NotEmpty().WithMessage("S? di?n tho?i là b?t bu?c")
            .Matches(@"^\d{10}$").WithMessage("S? di?n tho?i ph?i có 10 ch? s?");

        RuleFor(x => x.DateOfBirth)
            .LessThan(DateTime.Now.AddYears(-12)).WithMessage("H?i viên ph?i trên 12 tu?i");
    }
}
