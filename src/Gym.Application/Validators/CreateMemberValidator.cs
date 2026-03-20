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
            .NotEmpty().WithMessage("Họ và tên không được để trống")
            .MaximumLength(100).WithMessage("Họ và tên không quá 100 ký tự");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email là bắt buộc")
            .EmailAddress().WithMessage("Email không đúng định dạng");

        RuleFor(x => x.PhoneNumber)
            .NotEmpty().WithMessage("Số điện thoại là bắt buộc")
            .Matches(@"^\d{10}$").WithMessage("Số điện thoại phải có 10 chữ số");

        RuleFor(x => x.DateOfBirth)
            .LessThan(DateTime.Now.AddYears(-12)).WithMessage("Hội viên phải trên 12 tuổi");
    }
}