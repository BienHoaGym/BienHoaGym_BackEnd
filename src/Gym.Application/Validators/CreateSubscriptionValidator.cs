using FluentValidation;
using Gym.Application.DTOs.Subscriptions;

namespace Gym.Application.Validators.Subscriptions;

public class CreateSubscriptionValidator : AbstractValidator<CreateSubscriptionDto>
{
    public CreateSubscriptionValidator()
    {
        RuleFor(x => x.MemberId).NotEmpty().WithMessage("Phải chọn hội viên");
        RuleFor(x => x.PackageId).NotEmpty().WithMessage("Phải chọn gói tập");
        RuleFor(x => x.StartDate)
            .GreaterThanOrEqualTo(DateTime.UtcNow.Date.AddDays(-1)).WithMessage("Ngày bắt đầu không được ở quá khứ");
    }
}