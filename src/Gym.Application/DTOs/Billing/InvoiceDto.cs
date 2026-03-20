using Gym.Domain.Enums;

namespace Gym.Application.DTOs.Billing;

public class InvoiceDto
{
    public Guid Id { get; set; }
    public string InvoiceNumber { get; set; } = string.Empty;
    public Guid? MemberId { get; set; }
    public string? MemberName { get; set; }
    public decimal TotalAmount { get; set; }
    public decimal DiscountAmount { get; set; }
    public decimal FinalAmount { get; set; }
    public PaymentMethod PaymentMethod { get; set; }
    public PaymentStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public string? Note { get; set; }
    public List<InvoiceDetailDto> Details { get; set; } = new();
}

public class InvoiceDetailDto
{
    public Guid Id { get; set; }
    public string ItemType { get; set; } = string.Empty;
    public Guid? ReferenceId { get; set; }
    public string ItemName { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal Subtotal { get; set; }
}

public class CreateInvoiceDto
{
    public Guid? MemberId { get; set; }
    public decimal DiscountAmount { get; set; }
    public PaymentMethod PaymentMethod { get; set; }
    public string? Note { get; set; }
    public List<CreateInvoiceDetailDto> Details { get; set; } = new();
}

public class CreateInvoiceDetailDto
{
    public string ItemType { get; set; } = string.Empty; // Package, Product, PT_Session
    public Guid? ReferenceId { get; set; }
    public Guid? SubscriptionId { get; set; }
    public string ItemName { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
}
