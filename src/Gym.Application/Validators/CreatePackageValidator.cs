using FluentValidation;
using Gym.Application.DTOs.Packages;

namespace Gym.Application.Validators;

public class CreatePackageValidator : AbstractValidator<CreatePackageDto>
{
    public CreatePackageValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Ten goi tap khong duoc de trong")
            .MaximumLength(100).WithMessage("Ten goi khong qua 100 ky tu");

        RuleFor(x => x.Price)
            .GreaterThan(0).WithMessage("Gia goi tap phai lon hon 0");

        RuleFor(x => x.DurationInDays)
            .InclusiveBetween(1, 4000).WithMessage("Thoi han phai tu 1 den 4000 ngay");
    }
}
