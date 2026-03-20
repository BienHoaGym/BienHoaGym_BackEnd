using Gym.Domain.Common;

namespace Gym.Domain.Entities;

/// <summary>
/// Orders specialized for product selling (as per user's ERD request)
/// </summary>
public class Order : BaseEntity
{
    public string OrderNumber { get; set; } = string.Empty;
    public Guid? MemberId { get; set; }
    public virtual Member? Member { get; set; }

    public decimal TotalAmount { get; set; }
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    public string? Status { get; set; } // Completed, Cancelled...

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
}

public class OrderDetail : BaseEntity
{
    public Guid OrderId { get; set; }
    public virtual Order Order { get; set; } = null!;

    public Guid ProductId { get; set; }
    public virtual Product Product { get; set; } = null!;

    public int Quantity { get; set; }
    public decimal Price { get; set; } // Price at the time of purchase
    public decimal Subtotal => Quantity * Price;
}
