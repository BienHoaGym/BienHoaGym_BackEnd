using FluentValidation;
using Gym.Application.DTOs.Providers;

namespace Gym.Application.Validators;

public class CreateProviderValidator : AbstractValidator<CreateProviderDto>
{
    public CreateProviderValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Tên nhà cung cấp không được để trống")
            .MaximumLength(200).WithMessage("Tên nhà cung cấp không được quá 200 ký tự");

        RuleFor(x => x.Email)
            .EmailAddress().When(x => !string.IsNullOrEmpty(x.Email))
            .WithMessage("Email không đúng định dạng");

        RuleFor(x => x.PhoneNumber)
            .Matches(@"^[0-9+() -]{9,15}$").When(x => !string.IsNullOrEmpty(x.PhoneNumber))
            .WithMessage("Số điện thoại không hợp lệ");
            
        RuleFor(x => x.Code)
            .MaximumLength(50).WithMessage("Mã nhà cung cấp không được quá 50 ký tự");
    }
}
