using Gym.Domain.Enums;

namespace Gym.Application.DTOs.Inventory;

public class WarehouseDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? Location { get; set; }
    public bool IsActive { get; set; }
}

public class CreateWarehouseDto
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? Location { get; set; }
}

public class InventoryDto
{
    public Guid ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public string ProductSKU { get; set; } = string.Empty;
    public Guid WarehouseId { get; set; }
    public string WarehouseName { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public int MinStockThreshold { get; set; } // Ngưỡng tồn tối thiểu
    public decimal Price { get; set; } // Giá bán lẻ hiện tại
    public decimal CostPrice { get; set; } // Giá nhập hiện tại
    public DateTime LastUpdated { get; set; }
}

public class StockTransactionDto
{
    public Guid Id { get; set; }
    public Guid ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public Guid? FromWarehouseId { get; set; }
    public string? FromWarehouseName { get; set; }
    public Guid? ToWarehouseId { get; set; }
    public string? ToWarehouseName { get; set; }
    public StockTransactionType Type { get; set; }
    public int Quantity { get; set; }
    public int BeforeQuantity { get; set; }
    public int AfterQuantity { get; set; }
    public string? PerformedBy { get; set; }
    public decimal UnitPrice { get; set; }
    public DateTime Date { get; set; }
    public string? Note { get; set; }
    public string? ReferenceNumber { get; set; }
    public Guid? ProviderId { get; set; }
    public string? ProviderName { get; set; }
}

public class CreateStockTransactionDto
{
    public Guid ProductId { get; set; }
    public Guid? FromWarehouseId { get; set; }
    public Guid? ToWarehouseId { get; set; }
    public StockTransactionType Type { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal VatPercentage { get; set; } // Thuế VAT (%)
    public decimal PaidAmount { get; set; } // Số tiền thanh toán ngay
    public string? PaymentMethod { get; set; } // Tiền mặt/Chuyển khoản
    public DateTime? PaymentDueDate { get; set; } // Hạn thanh toán nợ
    public string? Note { get; set; }
    public string? ReferenceNumber { get; set; }
    public Guid? ProviderId { get; set; }
    public bool IsAsset { get; set; } // Phân biệt Hàng hóa vs Thiết bị
    public DateTime? ExpiryDate { get; set; } // Hạn sử dụng (Hàng hóa)
    public DateTime? TransactionDate { get; set; } // Ngày nhập thực tế
    public string? AttachmentUrl { get; set; } // Ảnh chụp chứng từ
    
    // Trường cho thiết bị
    public string? SerialNumber { get; set; }
    public DateTime? WarrantyExpiryDate { get; set; }
    public int? MaintenanceIntervalDays { get; set; }
}
