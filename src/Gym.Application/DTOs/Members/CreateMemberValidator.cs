using FluentValidation;
using Gym.Application.DTOs.Members;

namespace Gym.Application.Validators.Members;

public class CreateMemberValidator : AbstractValidator<CreateMemberDto>
{
    public CreateMemberValidator()
    {
        RuleFor(x => x.FullName)
            .NotEmpty().WithMessage("Họ Tên không được để trống")
            .MaximumLength(50).WithMessage("Họ TênTên không quá 50 ký tự");


        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email là bắt buộc")
            .EmailAddress().WithMessage("Định dạng email không hợp lệ");

        RuleFor(x => x.PhoneNumber)
            .NotEmpty().WithMessage("Số điện thoại là bắt buộc")
            .Matches(@"^\d{10,11}$").WithMessage("Số điện thoại không hợp lệ (phải là 10-11 số)");

        RuleFor(x => x.DateOfBirth)
            .LessThan(DateTime.Now.AddYears(-10)).WithMessage("Hội viên phải trên 10 tuổi");
    }
}