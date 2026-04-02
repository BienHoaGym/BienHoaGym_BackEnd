using AutoMapper;
using Gym.Application.DTOs.Payments;
using Gym.Domain.Entities;
using Gym.Domain.Enums;

namespace Gym.Application.Mappings;

public class PaymentProfile : Profile
{
    public PaymentProfile()
    {
        // 1. Entity -> PaymentDto (Hi?n th? ra danh sách)
        CreateMap<Payment, PaymentDto>()
            .ForMember(dest => dest.PaymentMethod, opt => opt.MapFrom(src => src.Method.ToString()))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
            // Map tên h?i viên t? quan h? l?ng nhau
            .ForMember(dest => dest.MemberName, opt => opt.MapFrom(src =>
                (src.Subscription != null && src.Subscription.Member != null)
                ? src.Subscription.Member.FullName : "N/A"))
            // Map tên gói t?p
            .ForMember(dest => dest.PackageName, opt => opt.MapFrom(src =>
                (src.Subscription != null && src.Subscription.Package != null)
                ? src.Subscription.Package.Name : "N/A"));

        // 2. ProcessPaymentDto -> Entity (Khi t?o thanh toán)
        CreateMap<ProcessPaymentDto, Payment>()
            .ForMember(dest => dest.Method, opt => opt.MapFrom(src => ParsePaymentMethod(src.PaymentMethod)))
            .ForMember(dest => dest.Note, opt => opt.MapFrom(src => src.Notes))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => PaymentStatus.Completed));

        // 3. CreatePaymentDto -> Entity (D? phòng)
        CreateMap<CreatePaymentDto, Payment>()
            .ForMember(dest => dest.Method, opt => opt.MapFrom(src => ParsePaymentMethod(src.PaymentMethod)))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => PaymentStatus.Completed))
            .ForMember(dest => dest.PaymentDate, opt => opt.MapFrom(src => DateTime.UtcNow));
    }

    private PaymentMethod ParsePaymentMethod(string method)
    {
        if (Enum.TryParse<PaymentMethod>(method, true, out var result))
        {
            return result;
        }
        return PaymentMethod.Cash;
    }
}
