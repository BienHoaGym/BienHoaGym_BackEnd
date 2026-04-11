namespace Gym.Application.DTOs.Packages;

public class CreatePackageDto
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public decimal? DiscountPrice { get; set; }
    public int DurationInDays { get; set; }
    public int DurationInMonths { get; set; }
    public int? SessionLimit { get; set; }
    public bool HasPT { get; set; } // "Có PT hay không có PT"
    public bool IsActive { get; set; } = true;
}
