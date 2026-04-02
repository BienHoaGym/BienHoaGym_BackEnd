using FluentValidation;
using Gym.Application.DTOs.Packages;

namespace Gym.Application.Validators.Packages;

public class CreatePackageValidator : AbstractValidator<CreatePackageDto>
{
    public CreatePackageValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Tên gói t?p không du?c d? tr?ng")
            .MaximumLength(100).WithMessage("Tên gói không quá 100 ký t?");

        RuleFor(x => x.Price)
            .GreaterThan(0).WithMessage("Giá gói t?p ph?i l?n hon 0");

        RuleFor(x => x.DurationDays)
            .InclusiveBetween(1, 365).WithMessage("Th?i h?n ph?i t? 1 d?n 365 ngày");
    }
}
