using Gym.Domain.Common;
using Gym.Domain.Enums;

namespace Gym.Domain.Entities;

/// <summary>
/// Invoice entity - Hóa đơn bán hàng
/// </summary>
public class Invoice : BaseEntity
{
    public string InvoiceNumber { get; set; } = string.Empty; // HD-20240001
    public Guid? MemberId { get; set; }
    public virtual Member? Member { get; set; }

    public decimal TotalAmount { get; set; }
    public decimal DiscountAmount { get; set; }
    public decimal FinalAmount => TotalAmount - DiscountAmount;

    public PaymentMethod PaymentMethod { get; set; }
    public PaymentStatus Status { get; set; } = PaymentStatus.Completed;
    
    public string? Note { get; set; }

    public virtual ICollection<InvoiceDetail> Details { get; set; } = new List<InvoiceDetail>();
}

public class InvoiceDetail : BaseEntity
{
    public Guid InvoiceId { get; set; }
    public virtual Invoice Invoice { get; set; } = null!;

    public string ItemType { get; set; } = string.Empty; // Package, Product, PT_Session
    public Guid? ReferenceId { get; set; } // Id của Package hoặc Product
    public Guid? SubscriptionId { get; set; } // Id của Gói tập (nếu thanh toán cho gói Pending)
    public string ItemName { get; set; } = string.Empty;
    
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal Subtotal => Quantity * UnitPrice;
}
