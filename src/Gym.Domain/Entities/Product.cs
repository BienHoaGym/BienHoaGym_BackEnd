using Gym.Domain.Common;

namespace Gym.Domain.Entities;

/// <summary>
/// Product entity - Sản phẩm (Nước uống, thực phẩm chức năng, phụ kiện...)
/// </summary>
public class Product : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string SKU { get; set; } = string.Empty; // Mã sản phẩm
    public decimal Price { get; set; }
    public int StockQuantity { get; set; }
    public int MinStockThreshold { get; set; } = 5; // Cảnh báo khi dưới mức này
    public DateTime? ExpirationDate { get; set; } // Hạn sử dụng (cho thực phẩm/TPBS)
    public string? Category { get; set; }
    public string? ImageUrl { get; set; }
    public Guid? ProviderId { get; set; }
    public virtual Provider? Provider { get; set; }
    public bool IsActive { get; set; } = true;
}
