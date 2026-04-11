using Gym.Domain.Common;
using Gym.Domain.Enums;
using System;
using System.Collections.Generic;

namespace Gym.Domain.Entities;

/// <summary>
/// Stock Audit entity - Phiếu kiểm kê kho
/// </summary>
public class StockAudit : BaseEntity
{
    public Guid WarehouseId { get; set; }
    public virtual Warehouse Warehouse { get; set; } = null!;

    public DateTime AuditDate { get; set; } = DateTime.UtcNow;
    public string? PerformedBy { get; set; }
    public string? ApprovedBy { get; set; }
    public StockAuditStatus Status { get; set; } = StockAuditStatus.Draft;
    
    public string? Note { get; set; }
    
    public virtual ICollection<StockAuditDetail> Details { get; set; } = new List<StockAuditDetail>();
}

public class StockAuditDetail : BaseEntity
{
    public Guid StockAuditId { get; set; }
    
    [System.Text.Json.Serialization.JsonIgnore]
    public virtual StockAudit StockAudit { get; set; } = null!;

    public Guid ProductId { get; set; }
    public virtual Product Product { get; set; } = null!;

    public int SystemQuantity { get; set; } // Số lượng trên hệ thống
    public int ActualQuantity { get; set; } // Số lượng đếm thực tế
    public int Difference => ActualQuantity - SystemQuantity; // Chênh lệch
    
    public string? Reason { get; set; } // Lý do chênh lệch
}

public enum StockAuditStatus
{
    Draft = 1,      // Đang đếm
    Pending = 2,    // Chờ duyệt
    Approved = 3,   // Đã duyệt (Đã cập nhật kho)
    Cancelled = 4   // Đã hủy
}
