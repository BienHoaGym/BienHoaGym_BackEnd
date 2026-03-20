using FluentValidation;
using Gym.Application.DTOs.Packages;

namespace Gym.Application.Validators.Packages;

public class CreatePackageValidator : AbstractValidator<CreatePackageDto>
{
    public CreatePackageValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Tên gói tập không được để trống")
            .MaximumLength(100).WithMessage("Tên gói không quá 100 ký tự");

        RuleFor(x => x.Price)
            .GreaterThan(0).WithMessage("Giá gói tập phải lớn hơn 0");

        RuleFor(x => x.DurationInDays)
            .InclusiveBetween(1, 365).WithMessage("Thời hạn phải từ 1 đến 365 ngày");
    }
}