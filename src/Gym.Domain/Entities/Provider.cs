using Gym.Domain.Common;
using Gym.Domain.Enums;

namespace Gym.Domain.Entities;

/// <summary>
/// Nhà cung cấp thiết bị, sản phẩm, dịch vụ
/// </summary>
public class Provider : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string? Code { get; set; } // Mã NCC (e.g. NCC001)
    public string? ContactPerson { get; set; }
    public string? Address { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Email { get; set; }
    public string? TaxCode { get; set; } // Thay cho VATCode để khớp SQL
    public string? SupplyType { get; set; } 
    public string? BankAccountNumber { get; set; }
    public string? BankName { get; set; }
    public string? Note { get; set; }
    public decimal TotalDebt { get; set; } // Tổng nợ hiện tại (Accounts Payable)
    public bool IsActive { get; set; } = true;

    // Relationships
    public virtual ICollection<Equipment> Equipments { get; set; } = new List<Equipment>();
    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
    public virtual ICollection<MaintenanceLog> MaintenanceLogs { get; set; } = new List<MaintenanceLog>();
    public virtual ICollection<StockTransaction> StockTransactions { get; set; } = new List<StockTransaction>();
}
