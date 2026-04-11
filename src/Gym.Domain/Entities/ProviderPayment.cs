using Gym.Domain.Common;

namespace Gym.Domain.Entities;

/// <summary>
/// Phiếu chi - Thanh toán nợ cho nhà cung cấp
/// </summary>
public class ProviderPayment : BaseEntity
{
    public Guid ProviderId { get; set; }
    public virtual Provider Provider { get; set; } = null!;

    public decimal Amount { get; set; }
    public string PaymentMethod { get; set; } = "Tiền mặt"; // Tiền mặt, Chuyển khoản...
    public DateTime Date { get; set; } = DateTime.UtcNow;
    public string? Note { get; set; }
    public string? ReferenceNumber { get; set; } // Mã chứng từ thanh toán (e.g. PC001)

    // Liên kết tới giao dịch cụ thể (nếu trả nợ cho đích danh 1 đơn)
    public Guid? StockTransactionId { get; set; }
    public virtual StockTransaction? StockTransaction { get; set; }

    public Guid? EquipmentTransactionId { get; set; }
    public virtual EquipmentTransaction? EquipmentTransaction { get; set; }
    
    public string? PerformedBy { get; set; } // Người thực hiện chi
}
