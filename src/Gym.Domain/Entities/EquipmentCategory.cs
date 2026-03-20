using Gym.Domain.Common;

namespace Gym.Domain.Entities;

/// <summary>
/// Danh mục loại thiết bị (Cardio, Tạ tay, Máy khối...)
/// </summary>
public class EquipmentCategory : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty; // Mã loại (e.g. CARDIO, STRENGTH)
    public string? Description { get; set; }
    public string? Group { get; set; } // Nhóm máy
    public decimal? AvgMaintenanceCost { get; set; }
    public int? StandardWarrantyMonths { get; set; }

    public virtual ICollection<Equipment> Equipments { get; set; } = new List<Equipment>();
}
