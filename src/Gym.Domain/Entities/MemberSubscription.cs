using Gym.Domain.Common;
using Gym.Domain.Enums;
using System;
using System.Collections.Generic;

namespace Gym.Domain.Entities;

public class MemberSubscription : BaseEntity
{
    public Guid MemberId { get; set; }
    public virtual Member? Member { get; set; }

    public Guid PackageId { get; set; }
    public virtual MembershipPackage? Package { get; set; }

    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public SubscriptionStatus Status { get; set; } = SubscriptionStatus.Pending;
    public int? RemainingSessions { get; set; }

    // Quan h? v?i thanh toán
    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

    // ? Ðã s?a l?i: Thêm danh sách Check-in
    public virtual ICollection<CheckIn> CheckIns { get; set; } = new List<CheckIn>();

    // --- NGHI?P V? I.3 (SNAPSHOT DATA) ---
    public string OriginalPackageName { get; set; } = string.Empty;
    public decimal OriginalPrice { get; set; }
    public decimal DiscountApplied { get; set; }
    public decimal FinalPrice { get; set; }
}
