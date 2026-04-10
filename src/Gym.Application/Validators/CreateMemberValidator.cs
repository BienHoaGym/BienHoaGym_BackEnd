using System;
using FluentValidation;
using Gym.Application.DTOs.Members;

namespace Gym.Application.Validators;

public class CreateMemberValidator : AbstractValidator<CreateMemberDto>
{
    public CreateMemberValidator()
    {
        RuleFor(x => x.FullName)
            .NotEmpty().WithMessage("Họ và tên không được để trống")
            .MaximumLength(100).WithMessage("Họ và tên không quá 100 ký tự");

        RuleFor(x => x.Email)
            .EmailAddress().When(x => !string.IsNullOrEmpty(x.Email))
            .WithMessage("Email không đúng định dạng");

        RuleFor(x => x.PhoneNumber)
            .NotEmpty().WithMessage("Số điện thoại là bắt buộc")
            .Matches(@"^0\d{9}$").WithMessage("Số điện thoại phải có 10 chữ số và bắt đầu bằng số 0");

        RuleFor(x => x.DateOfBirth)
            .LessThan(DateTime.UtcNow.AddYears(-12))
            .When(x => x.DateOfBirth.HasValue)
            .WithMessage("Hội viên phải trên 12 tuổi");
    }
}