using Gym.Domain.Common;
using Gym.Domain.Enums;

namespace Gym.Domain.Entities;

/// <summary>
/// Nhà cung cấp thiết bị, sản phẩm, dịch vụ
/// </summary>
public class Provider : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty; // Mã NCC (e.g. NCC001)
    public string? Address { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Email { get; set; }
    public string? VATCode { get; set; }
    public string? SupplyType { get; set; } // Thiết bị, dụng cụ, phụ kiện, thực phẩm...
    public string? BankAccountNumber { get; set; }
    public string? BankName { get; set; }
    public string? Note { get; set; }
    public bool IsActive { get; set; } = true;

    // Relationships
    public virtual ICollection<Equipment> Equipments { get; set; } = new List<Equipment>();
    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
    public virtual ICollection<MaintenanceLog> MaintenanceLogs { get; set; } = new List<MaintenanceLog>();
    public virtual ICollection<StockTransaction> StockTransactions { get; set; } = new List<StockTransaction>();
}
