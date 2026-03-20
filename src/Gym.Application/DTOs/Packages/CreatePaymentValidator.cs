using FluentValidation;
using Gym.Application.DTOs.Payments;

namespace Gym.Application.Validators.Payments;

public class CreatePaymentValidator : AbstractValidator<CreatePaymentDto>
{
    public CreatePaymentValidator()
    {
        RuleFor(x => x.MemberSubscriptionId).NotEmpty();
        RuleFor(x => x.Amount).GreaterThan(0).WithMessage("Số tiền thanh toán phải lớn hơn 0");
        RuleFor(x => x.PaymentMethod).NotEmpty().WithMessage("Vui lòng chọn phương thức thanh toán");
    }
}