namespace Gym.Application.DTOs.Inventory;

public class CreateOrderDto
{
    public Guid? MemberId { get; set; }
    public List<CreateOrderDetailDto> Details { get; set; } = new();
}

public class CreateOrderDetailDto
{
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
}
