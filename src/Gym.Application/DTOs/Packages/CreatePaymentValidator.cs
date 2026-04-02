using FluentValidation;
using Gym.Application.DTOs.Payments;

namespace Gym.Application.Validators.Payments;

public class CreatePaymentValidator : AbstractValidator<CreatePaymentDto>
{
    public CreatePaymentValidator()
    {
        RuleFor(x => x.MemberSubscriptionId).NotEmpty();
        RuleFor(x => x.Amount).GreaterThan(0).WithMessage("S? ti?n thanh toán ph?i l?n hon 0");
        RuleFor(x => x.PaymentMethod).NotEmpty().WithMessage("Vui lòng ch?n phuong th?c thanh toán");
    }
}
