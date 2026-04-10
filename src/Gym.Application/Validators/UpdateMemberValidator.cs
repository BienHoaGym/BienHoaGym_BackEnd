using FluentValidation;
using Gym.Application.DTOs.Members;
using System;
using System.Linq;

namespace Gym.Application.Validators.Members;

public class UpdateMemberValidator : AbstractValidator<UpdateMemberDto>
{
    public UpdateMemberValidator()
    {
        RuleFor(x => x.FullName)
           .NotEmpty().WithMessage("Họ và tên là bắt buộc")
           .MaximumLength(100).WithMessage("Họ và tên không được vượt quá 100 ký tự");
        
        RuleFor(x => x.Email)
            .EmailAddress().When(x => !string.IsNullOrEmpty(x.Email))
            .WithMessage("Định dạng email không hợp lệ");

        RuleFor(x => x.PhoneNumber)
            .NotEmpty().WithMessage("Số điện thoại là bắt buộc")
            .Matches(@"^0\d{9}$").WithMessage("Số điện thoại phải có 10 chữ số và bắt đầu bằng số 0");

        RuleFor(x => x.DateOfBirth)
            .LessThan(DateTime.Today).When(x => x.DateOfBirth.HasValue)
            .WithMessage("Ngày sinh phải trong quá khứ")
            .Must(BeAtLeast10YearsOld).When(x => x.DateOfBirth.HasValue)
            .WithMessage("Hội viên phải ít nhất 10 tuổi");

        RuleFor(x => x.Gender)
            .Must(g => string.IsNullOrEmpty(g) || new[] { "Male", "Female", "Other", "Nam", "Nữ", "Khác" }.Contains(g))
            .WithMessage("Giới tính không hợp lệ (Nam, Nữ, Khác)");

        RuleFor(x => x.EmergencyPhone)
            .Matches(@"^0\d{9}$").When(x => !string.IsNullOrEmpty(x.EmergencyPhone))
            .WithMessage("SĐT khẩn cấp phải có 10 chữ số và bắt đầu bằng số 0");
    }

    private bool BeAtLeast10YearsOld(DateTime? dateOfBirth)
    {
        if (!dateOfBirth.HasValue) return true;
        var age = DateTime.Today.Year - dateOfBirth.Value.Year;
        if (dateOfBirth.Value.Date > DateTime.Today.AddYears(-age)) age--;
        return age >= 10;
    }
}